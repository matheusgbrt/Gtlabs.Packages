using Gtlabs.Persistence.Entities;

namespace Gtlabs.Persistence.Repository;

public interface IRepository<T> where T : AuditedEntity
{
    public IQueryable<T> Query();
    Task<T?> GetByIdAsync(Guid id, bool asNoTracking = true);
    Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = true);
    Task InsertAsync(T entity, bool autoSave = false);
    Task UpdateAsync(T entity, bool autoSave = false);
    Task DeleteAsync(T entity, bool autoSave = false);

}