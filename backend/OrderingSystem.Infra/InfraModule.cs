using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderingSystem.Infra.Data;

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

    return services;
  }
}
