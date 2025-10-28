using System.Text.Json;
using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;
using Gtlabs.Redis.Interfaces;
using StackExchange.Redis;

namespace Gtlabs.Redis.Services;

internal class CacheService<T> : ICacheService<T>, IScopedDependency
{
    private readonly IDatabase _database;

    public CacheService(RedisConnectionManager connectionManager)
    {
        _database = connectionManager.GetDatabase();
    }

    public async Task SetAsync(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, json, expiry);
    }

    public async Task<T?> GetAsync(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return default;

        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task<bool> DeleteAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }
}