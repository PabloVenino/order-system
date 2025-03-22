
using OrderingSystem.Domain.Entities;

namespace OrderingSystem.Application.Interfaces;

public interface IOrderRepository
{
  Task<IEnumerable<Order>> GetPendingOrders();
}