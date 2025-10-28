# Gtlabs.Redis

Utilities and abstractions for working with Redis in .NET applications. Provides a connection manager, a cache service interface, and DI extensions for easy integration.

## Features

- **Redis connection management**: Centralized management of Redis connections.
- **Cache service abstraction**: `ICacheService` interface and implementation for common cache operations.
- **DI integration**: Extension methods to register Redis services in the dependency injection container.

## Usage

Add the package to your project and register the Redis cache service:

```csharp
services.AddRedisCache();
```

Use `ICacheService<T>` in your services or controllers:

```csharp
public class MyService
{
    private readonly ICacheService<CachedUser> _cache;

    public MyService(ICacheService<CachedUser> cache)
    {
        _cache = cache;
    }

    public async Task SetUserCache(CachedUser user)
    {
        await _cache.SetAsync(user.Id, user);
        var value = await _cache.GetAsync<string>(user.Id.ToString());
    }
}
```

## Required environment variables

- `REDIS_CONNECTION_STRING`: Redis server connection string (e.g., `localhost:6379`).
- `REDIS_DEFAULT_DB`: Redis database to connect to.