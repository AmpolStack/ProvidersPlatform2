using System.Linq.Expressions;

namespace ProvidersPlatform.Shared.Setup.Defaults;

public interface IGenericRepository<T> where T : class
{
    public Task<T?> GetEntity(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    public Task<IQueryable<T>> GetEntities(Expression<Func<T, bool>>? predicate = null);
    public Task<bool> Update(T entity, CancellationToken cancellationToken = default);
    public Task<bool> Delete(T entity, CancellationToken cancellationToken = default);
    public Task<T> Create(T entity, CancellationToken cancellationToken = default);
}