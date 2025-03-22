using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace OrderingSystem.Application;

public static class ApplicationModule
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    return services;
  }
}
