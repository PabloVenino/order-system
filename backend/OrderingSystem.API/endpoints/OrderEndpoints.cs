
using OrderingSystem.Application.Interfaces;
using OrderingSystem.Domain.Entities;

namespace OrderingSystem.API.Endpoints;

internal static class OrderEndpoints
{
  public static void RegisterOrderSystemEndpoints(this WebApplication app)
  {
    RouteGroupBuilder group = app.MapGroup("/api/v1/orders");

    // Mapping endpoints
    group.MapGet("/", async (IOrderRepository orderRepo) =>
    {
      var orders = await orderRepo.GetAllAsync();
      return Results.Ok(orders);
    });

    group.MapGet("/{id:guid}", async (Guid id, IOrderRepository orderRepo) =>
    {
      var order = await orderRepo.GetByIdAsync(id);
      return order is null ? Results.Ok(order) : Results.NotFound();
    });

    group.MapPost("/", async (Order order, IOrderRepository orderRepo) =>
    {
      await orderRepo.AddAsync(order);
      return Results.Ok(order.Id);
    });

    group.MapPut("/{id:guid}", async (Guid id, Order newOrder, IOrderRepository orderRepo) =>
    {
      var currentOrder = await orderRepo.GetByIdAsync(id);
      if (currentOrder is null) return Results.NotFound();

      currentOrder.Status = newOrder.Status;
      orderRepo.Update(currentOrder);

      return Results.Ok(currentOrder.Id);
    });

    group.MapDelete("/{id:guid}", async (Guid id, IOrderRepository orderRepo) =>
    {
      var order = await orderRepo.GetByIdAsync(id);
      if (order is null) return Results.NotFound();

      orderRepo.Delete(order);
      return Results.Ok();
    });


  }
}