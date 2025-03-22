using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderingSystem.Application.Interfaces;
using OrderingSystem.Infra.Data;
using OrderingSystem.Infra.Repositories;

namespace OrderingSystem.Infra;

public static class InfraModule
{
  public static IServiceCollection AddInfra(
    this IServiceCollection services,
    IConfigurationManager config
  )
  {
    services.AddDbContext<AppDbContext>(opt =>
      opt.UseNpgsql(config.GetConnectionString("DefaultConnection"))
    );

    services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));
    services.AddScoped<IOrderRepository, OrderRepository>();

    return services;
  }
}
