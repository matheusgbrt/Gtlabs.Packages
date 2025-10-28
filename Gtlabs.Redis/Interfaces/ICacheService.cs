namespace Gtlabs.Redis.Interfaces;

public interface ICacheService<T>
{
    Task SetAsync(string key, T value, TimeSpan? expiry = null);
    Task<T?> GetAsync(string key);
    Task<bool> DeleteAsync(string key);
}