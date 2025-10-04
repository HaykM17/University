using Application.Repositories.GenericRepository;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.GenericRepository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _appDbContext;

    public GenericRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _appDbContext.Set<T>().AddAsync(entity, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public  IQueryable<T> GetAll(CancellationToken cancellationToken = default)
    {
        return _appDbContext.Set<T>()
            .AsNoTracking();
            
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        //return await _appDbContext.Set<T>().FindAsync([ id ] , cancellationToken);

        return await _appDbContext.Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id, cancellationToken);
    }

    public async Task<T?> UpdateFullAsync(int id, T entity, CancellationToken cancellationToken = default)
    {

        var exists = await _appDbContext.Set<T>().AnyAsync(e => EF.Property<int>(e, "Id") == id, cancellationToken);

        if (!exists)
        {
            return null;
        }

        var tracked = _appDbContext.Set<T>().Local.FirstOrDefault(e =>
            Equals(_appDbContext.Entry(e).Property("Id").CurrentValue, id));

        if (tracked != null)
        {
            _appDbContext.Entry(tracked).State = EntityState.Detached;
        }

        _appDbContext.Entry(entity).Property("Id").CurrentValue = id;
        _appDbContext.Update(entity);
        _appDbContext.Entry(entity).Property("Id").IsModified = false;

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<T?> UpdatePartialAsync(int id, T entity, CancellationToken cancellationToken = default)
    {
        var existing = await _appDbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);

        if (existing == null)
        {
            return null;
        }

        var entry = _appDbContext.Entry(existing);
        var src = _appDbContext.Entry(entity);

        entry.Property("Id").IsModified = false;
        
        entry.Property("FirstName").CurrentValue = src.Property("FirstName").CurrentValue;
        entry.Property("FirstName").IsModified = true;
                
        entry.Property("LastName").CurrentValue = src.Property("LastName").CurrentValue;
        entry.Property("LastName").IsModified = true;
        
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<Dictionary<int, List<int>>> UpdateBulkAsync(List<T> entities, CancellationToken cancellationToken = default)
    {
        var dic = new Dictionary<int, List<int>>();

        if (entities == null || entities.Count == 0)
        {
            dic.Add(0, new List<int>());

            return dic;
        }

        var ids = entities
            .Select(x => (int)_appDbContext.Entry(x).Property("Id").CurrentValue!)
            .ToList();

        var existingList = await _appDbContext.Set<T>()
            .Where(e => ids.Contains(EF.Property<int>(e, "Id")))
            .ToListAsync(cancellationToken);

        var existingById = existingList.ToDictionary(
            e => (int)_appDbContext.Entry(e).Property("Id").CurrentValue!, e => e);

        var notFound = new List<int>();
        var updated = 0;

        foreach (var entity in entities)
        {
            var id = (int)_appDbContext.Entry(entity).Property("Id").CurrentValue!;

            if (!existingById.TryGetValue(id, out var target))
            {
                notFound.Add(id);
                continue;
            }

            _appDbContext.Entry(target).CurrentValues.SetValues(entity);

            _appDbContext.Entry(target).Property("Id").IsModified = false;

            updated++;
        }

        await _appDbContext.SaveChangesAsync(cancellationToken);

        dic[updated] = notFound;

        return dic;
    }

    public async Task<T?> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existingEntity = await _appDbContext.Set<T>()
            .FirstOrDefaultAsync(x => EF.Property<int?>(x, "Id") == id, cancellationToken);

        if (existingEntity == null)
        {
            return null;
        }

        _appDbContext.Set<T>().Remove(existingEntity);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        return existingEntity;
    }

    public async Task<int> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        var list = entities?.ToList() ?? [];

        if (list.Count == 0)
        {
            return 0;
        }

        await _appDbContext.Set<T>().AddRangeAsync(list, cancellationToken);
        return await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> DeleteFromListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var toRemove = await _appDbContext.Set<T>().Where(predicate).ToListAsync(cancellationToken);

        if (toRemove.Count == 0)
        {
            return 0;
        }

        _appDbContext.Set<T>().RemoveRange(toRemove);
        return await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}