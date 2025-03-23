
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Application.Interfaces;
using OrderingSystem.Domain.Constants;
using OrderingSystem.Domain.Entities;
using OrderingSystem.Infra.Data;

namespace OrderingSystem.Infra.Repositories;

public sealed class OrderRepository(AppDbContext context) : EntityRepository<Order>(context), IOrderRepository
{
  public async Task<IEnumerable<Order>> GetPendingOrders()
  {
    return await _dbSet.Where(o => o.Status == OrderStatus.PENDING).ToListAsync();
  }
}