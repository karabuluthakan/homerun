using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Repository;

public interface IRepository<T,TKey> where T : Entity<TKey>, new()
{
    ValueTask<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    ValueTask<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    ValueTask<Exception?> AddAsync(T entity, CancellationToken cancellationToken = default);
    ValueTask<Exception?> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    ValueTask<Exception?> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
}