using OrderingSystem.Domain.Constants;
using OrderingSystem.Domain.Entities;
using OrderingSystem.Domain.Models;

namespace OrderingSystem.Domain.Extensions
{
  public static class OrderExtensions
  {
    public static Order ToEntity(this OrderDto dto)
    {
      return new Order()
      {
        Id = new Guid(),
        Product = dto.Product,
        Customer = dto.Customer,
        Value = dto.Value,
        CreatedAt = DateTime.UtcNow,
        Status = OrderStatus.PENDING
      };
    }

    public static OrderDto ToDto(this Order order)
    {
      return new OrderDto
      {
        Customer = order.Customer,
        Product = order.Product,
        Value = order.Value
      };
    }
  }
}