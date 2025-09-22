using Application.Common.Results;
using System.Linq.Expressions;

namespace Application.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    IQueryable<T> GetAll(CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<T?> UpdateFullAsync(int id, T entity, CancellationToken cancellationToken = default);
    Task<T?> UpdatePartialAsync(int id, T entity, CancellationToken cancellationToken = default);
    Task<BulkUpdateResult> UpdateBulkAsync (List<T> entities, CancellationToken cancellationToken = default);
    Task<T?> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<int> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task<int> DeleteFromListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
}