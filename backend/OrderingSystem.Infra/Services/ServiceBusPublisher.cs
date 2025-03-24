
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using OrderingSystem.Application.Interfaces.Messaging;

namespace OrderingSystem.Infra.Services.Messaging;

public class ServiceBusPublisher : IServiceBusPublisher
{
  private readonly ServiceBusClient _client;
  private readonly ServiceBusSender _sender;
  public ServiceBusPublisher(string connectionString, string queueName)
  {
    _client = new ServiceBusClient(connectionString);
    _sender = _client.CreateSender(queueName);
  }

  public async Task PublishAsync(object message)
  {
    var jsonMessage = JsonSerializer.Serialize(message);
    var serviceBusMessage = new ServiceBusMessage(jsonMessage);

    await _sender.SendMessageAsync(serviceBusMessage);
  }
}