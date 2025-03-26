
using OrderingSystem.Application.Interfaces;
using OrderingSystem.Application.Interfaces.Messaging;
using OrderingSystem.Domain.Constants;
using OrderingSystem.Domain.Entities;
using OrderingSystem.Domain.Extensions;
using OrderingSystem.Domain.Models;

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
      if (orders is not null)
      {
        List<OrderDto> dtos = [];
        foreach (var order in orders)
        {
          dtos.Add(order.ToDto());
        }

        return Results.Ok(
          new Response<List<OrderDto>>
          {
            Data = dtos,
            ErrorDetails = null,
            Message = "Pedidos obtidos com sucesso."
          }
        );
      }
      else
      {
        return Results.NotFound(
          new Response<List<OrderDto>>
          {
            Data = null,
            ErrorDetails = null,
            Message = "Pedidos não encontrados."
          }
        );
      }
    });

    group.MapGet("/{id:guid}", async (Guid id, IOrderRepository orderRepo) =>
    {
      var order = await orderRepo.GetByIdAsync(id);
      if (order is not null)
      {
        var dto = order?.ToDto();

        return Results.Ok(
          new Response<OrderDto>
          {
            Data = dto,
            Message = "Pedido obtido com sucesso.",
            ErrorDetails = null
          }
        );
      }
      else
      {
        return Results.NotFound(
          new Response<OrderDto>
          {
            Data = null,
            Message = "Pedido não encontrado.",
            ErrorDetails = new Error
            {
              ErrorCode = ErrorCodes.ORDER_NOT_FOUND,
              IsError = true
            }
          }
        );
      }
    });

    group.MapPost("/", async (OrderDto orderDto, IOrderRepository orderRepo, IServiceBusPublisher serviceBus) =>
    {
      var order = await orderRepo.AddAsync(orderDto.ToEntity());

      await serviceBus.PublishAsync(new { order.Id, order.Status, order.CreatedAt });

      return Results.Ok(
        new Response<OrderDto>
        {
          Data = orderDto,
          Message = "Pedido inserido com sucesso.",
          ErrorDetails = null
        }
      );
    });

    group.MapPut("/{id:guid}", async (Guid id, OrderDto newOrder, IOrderRepository orderRepo) =>
    {
      var currentOrder = await orderRepo.GetByIdAsync(id);
      if (currentOrder is null)
        return Results.NotFound(new Response<OrderDto>
        {
          Data = null,
          ErrorDetails = new Error
          {
            ErrorCode = ErrorCodes.ORDER_NOT_FOUND,
            IsError = true
          },
          Message = "Pedido não encontrado."
        });

      orderRepo.Update(currentOrder);
      var dto = currentOrder.ToDto();
      return Results.Ok(new Response<OrderDto> { Data = dto, Message = "Pedido editado com sucesso." });
    });

    group.MapDelete("/{id:guid}", async (Guid id, IOrderRepository orderRepo) =>
    {
      var order = await orderRepo.GetByIdAsync(id);
      if (order is null) 
        return Results.NotFound(new Response<OrderDto>{
          Data = null,
          Message = "Pedido não encontrado.",
          ErrorDetails = new Error{
            ErrorCode = ErrorCodes.ORDER_NOT_FOUND,
            IsError = true
          }
        });

      orderRepo.Delete(order);
      return Results.Ok(new Response<object> {
        Data = new { deletedOrder = order.Id },
        ErrorDetails = null,
        Message = $"Pedido numero {order.Id} deletado com sucesso."
      });
    });
  }
}