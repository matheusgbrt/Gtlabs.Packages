using System.Text.Json;
using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;
using Gtlabs.Redis.Abstractions;
using Gtlabs.Redis.Interfaces;
using StackExchange.Redis;

namespace Gtlabs.Redis.Services;

internal class CacheService<T> : ICacheService<T> where T : CacheEntity
{
    private readonly IDatabase _database;

    public CacheService(RedisConnectionManager connectionManager)
    {
        _database = connectionManager.GetDatabase();
    }

    public async Task SetAsync(T entity, TimeSpan? expiry = null)
    {
        var key = entity.BuildKey();
        var json = JsonSerializer.Serialize(entity);
        await _database.StringSetAsync(key, json, expiry);
    }

    public async Task<T?> GetAsync(T entity)
    {
        var key = entity.BuildKey();
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return null;

        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        var key = entity.BuildKey();
        return await _database.KeyDeleteAsync(key);
    }
}