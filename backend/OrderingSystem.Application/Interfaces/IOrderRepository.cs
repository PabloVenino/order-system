
using OrderingSystem.Domain.Entities;

namespace OrderingSystem.Application.Interfaces;

public interface IOrderRepository : IEntityRepository<Order>
{
  Task<IEnumerable<Order>> GetPendingOrders();
}