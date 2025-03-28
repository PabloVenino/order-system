
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Application.Interfaces;
using OrderingSystem.Infra.Data;

namespace OrderingSystem.Infra.Repositories;

public class EntityRepository<T>(AppDbContext context) : IEntityRepository<T> where T : class
{
  protected readonly AppDbContext _context = context;
  protected readonly DbSet<T> _dbSet = context.Set<T>();

  public async Task<T?> GetByIdAsync(Guid id)
  {
    return await _dbSet.FindAsync(id);
  }

  public async Task<IEnumerable<T>> GetAllAsync()
  {
    return await _dbSet.ToListAsync();
  }

  public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
  {
    return await _dbSet.Where(predicate).ToListAsync();
  }

  public async Task<T> AddAsync(T entity)
  {
    await _dbSet.AddAsync(entity);
    await SaveChangesAsync();
    return entity;
  }

  public async void Update(T entity)
  {
    _dbSet.Update(entity);
    await SaveChangesAsync();
  }

  public async void Delete(T entity)
  {
    _dbSet.Remove(entity);
    await SaveChangesAsync();
  }

  public async Task SaveChangesAsync()
  {
    await _context.SaveChangesAsync();
  }
}