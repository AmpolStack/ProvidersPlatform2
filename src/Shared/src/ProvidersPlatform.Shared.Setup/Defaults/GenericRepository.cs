using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProvidersPlatform.Shared.Models;
using ProvidersPlatform.Shared.Setup.Contexts;

namespace ProvidersPlatform.Shared.Setup.Defaults;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ProvidersPlatformContext _context;

    public GenericRepository(ProvidersPlatformContext context)
    {
        _context = context;
    }
    
    public async Task<T?> GetEntity(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var query = await _context.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
        return query;
    }
    
    public Task<IQueryable<T>> GetEntities(Expression<Func<T, bool>>? predicate = null)
    {
        var query = (predicate is null) ? _context.Set<T>() : _context.Set<T>().Where(predicate).AsQueryable();
        return Task.FromResult(query);
    }

    public async Task<bool> Update(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Delete(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<T> Create(T entity, CancellationToken cancellationToken = default)
    {
        var response = await _context.Set<T>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return response.Entity;
    }
}