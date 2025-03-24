using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderingSystem.Application.Interfaces;
using OrderingSystem.Application.Interfaces.Messaging;
using OrderingSystem.Infra.Data;
using OrderingSystem.Infra.Repositories;
using OrderingSystem.Infra.Services.Messaging;

namespace OrderingSystem.Infra;

public static class InfraModule
{
  public static IServiceCollection AddInfra(
    this IServiceCollection services,
    IConfigurationManager config
  )
  {
    string? queueName = config.GetSection("Azure")["QueueName"];

    services.AddDbContext<AppDbContext>(opt =>
      opt.UseNpgsql(config.GetConnectionString("DefaultConnection"))
    );

    services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));
    services.AddScoped<IOrderRepository, OrderRepository>();

    services.AddSingleton<IServiceBusPublisher>(new ServiceBusPublisher(config?.GetConnectionString("AzureServiceBus") ?? "", queueName!));

    return services;
  }
}
