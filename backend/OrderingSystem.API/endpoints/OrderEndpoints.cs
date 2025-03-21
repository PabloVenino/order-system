
namespace OrderingSystem.API.Endpoints;

internal static class OrderEndpoints {
  public static void RegisterOrderSystemEndpoints(this WebApplication app) {
    RouteGroupBuilder group = app.MapGroup("/orders");

    // Mapping endpoints
    group.MapGet("/", () => {
      Results.Ok("Hello, World!");
    });
  }
}