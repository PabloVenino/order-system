using System.Text.Json;
using Azure.Messaging.ServiceBus;
using OrderingSystem.Application.Interfaces;
using OrderingSystem.Domain.Constants;
using OrderingSystem.Domain.Entities;
using OrderingSystem.Domain.Models;

namespace OrderingSystem.Background;

public class OrderProcessingBackgroungService : BackgroundService
{
  private readonly ILogger<OrderProcessingBackgroungService> _logger;
  private readonly IServiceProvider _serviceProvider;
  readonly ServiceBusClient client;
  readonly ServiceBusProcessor _processor;

  public OrderProcessingBackgroungService(
    ILogger<OrderProcessingBackgroungService> logger,
    IServiceProvider serviceProvider,
    string connectionString
  )
  {
    _logger = logger;
    _serviceProvider = serviceProvider;
    var client = new ServiceBusClient(connectionString);
    _processor = client.CreateProcessor("update-order", "", new ServiceBusProcessorOptions());
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    _processor.ProcessMessageAsync += ProcessMessageAsync;
    // while (!stoppingToken.IsCancellationRequested)
    // {
    //   if (_logger.IsEnabled(LogLevel.Information))
    //   {
    //     _logger.LogInformation("OrderProcessingBackgroungService running at: {time}", DateTimeOffset.Now);
    //   }
    //   await Task.Delay(1000, stoppingToken);
    // }
  }

  private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
  {
    var jsonMessage = args.Message.Body.ToString();
    var orderData = JsonSerializer.Deserialize<Order>(jsonMessage);

    if (orderData is not null)
    {
      using var scope = _serviceProvider.CreateScope();
      var orderRepo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

      var order = await orderRepo.GetByIdAsync(orderData.Id);
      if (order is not null)
      {
        order.Status = OrderStatus.PROCESSING;
        orderRepo.Update(order);

        await Task.Delay(TimeSpan.FromSeconds(5));

        order.Status = OrderStatus.COMPLETED;
        orderRepo.Update(order);

        _logger.LogInformation($"Pedido {order.Id} processado.");
      }
    }

    await args.CompleteMessageAsync(args.Message);
  }

  private Task ErrorHandling(ProcessErrorEventArgs args)
  {
    _logger.LogError(args.Exception, "Erro ao executar Service Bus");
    return Task.CompletedTask;
  }

  public override async Task StopAsync(CancellationToken cancellationToken)
  {
    await _processor.StopProcessingAsync(cancellationToken);
    await _processor.DisposeAsync();
    await base.StopAsync(cancellationToken);
  }
}
