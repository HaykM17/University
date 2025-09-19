using Application.Dtos.Response;
using System.Linq.Expressions;

namespace Application.Abstract.Repasitories;

public interface IGenericRepository<T> where T : class
{
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    IQueryable<T> GetAll(CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<T?> UpdateFullAsync(int id, T entity, CancellationToken cancellationToken = default);
    Task<T?> UpdatePartialAsync(int id, T entity, CancellationToken cancellationToken = default);
    Task<BulkUpdateDto> UpdateBulkAsync (List<T> entities, CancellationToken cancellationToken = default);
    Task<T?> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<int> AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
    Task<int> DeleteFromListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
}