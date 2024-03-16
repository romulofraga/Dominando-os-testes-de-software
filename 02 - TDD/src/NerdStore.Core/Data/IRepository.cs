using System.Linq.Expressions;

namespace NerdStore.Core;

public interface IRepository<T> : IDisposable where T : IAggregateRoot
{
  IUnitOfWork UnitOfWork { get; }
  void Add(T entity);
  void Update(T entity);
  void Remove(T entity);
  Task<T> GetById(Guid id);
  Task<IEnumerable<T>> GetAll();
  Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
}