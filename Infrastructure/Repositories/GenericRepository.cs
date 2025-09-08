using Application.Abstract.Repasitories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _appDbContext;

    public GenericRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
    {
        await _appDbContext.Set<T>().AddAsync(entity, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _appDbContext.Set<T>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        //return await _appDbContext.Set<T>().FindAsync(new object[] { id } , cancellationToken);

        return await _appDbContext.Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id, cancellationToken);
    }

    public async Task<T?> UpdateAsync(int id, T entity, CancellationToken cancellationToken)
    {
        var existing = await _appDbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);

        //if (existing is null)
        //{
        //    return null;
        //}
        //_appDbContext.Entry(existing).CurrentValues.SetValues(entity);
        //_appDbContext.Entry(existing).Property("Id").IsModified = false;

        //await _appDbContext.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task<T?> DeleteAsync(int id, CancellationToken cancellationToken)
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

    //public async Task<T?> UpdateAsync(int id, T entity, CancellationToken cancellationToken)
    //{
    //    var exist = await _appDbContext.Set<T>()
    //        .AnyAsync(x => EF.Property<int>(x, "Id") == id, cancellationToken);

    //    if (!exist)
    //    {
    //        return null!;
    //    }

    //    _appDbContext.Update<T>(entity);
    //    await _appDbContext.SaveChangesAsync(cancellationToken);

    //    return entity;
    //}
}