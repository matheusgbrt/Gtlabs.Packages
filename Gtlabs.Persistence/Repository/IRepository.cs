using Gtlabs.Persistence.Entities;

namespace Gtlabs.Persistence.Repository;

public interface IRepository<T> where T : AuditedEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task SaveAsync(T entity);
    Task DeleteAsync(T entity);
    Task DeleteAsync(Guid id);
    Task PurgeAsync(T entity);
}