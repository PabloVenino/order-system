
using System.Linq.Expressions;

namespace OrderingSystem.Application.Interfaces;

public interface IEntityRepository<T> where T : class
{
  Task<T?> GetByIdAsync(Guid id);
  Task<IEnumerable<T>> GetAllAsync();
  Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
  Task<T> AddAsync(T entity);
  void Update(T entity);
  void Delete(T entity);
  Task SaveChangesAsync();
}