namespace Gtlabs.Redis.Interfaces;

public interface ICacheService<T>
{
    Task SetAsync(T entity, TimeSpan? expiry = null);
    Task<T?> GetAsync(T entity);
    Task<bool> DeleteAsync(T entity);
}