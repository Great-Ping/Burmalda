using System.Linq.Expressions;

namespace Burmalda.DataAccess;

public interface IRepository<T>
{
    Task<T> AddAsync(T target, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T target, CancellationToken cancellationToken = default);
    Task DeleteAsync(T target, CancellationToken cancellationToken = default);
    Task<T?> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<T[]> WhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

}