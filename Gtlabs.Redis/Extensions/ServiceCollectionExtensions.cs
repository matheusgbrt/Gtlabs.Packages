using Gtlabs.Redis.Interfaces;
using Gtlabs.Redis.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Redis.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedisCache(this IServiceCollection services)
    {
        services.AddSingleton<RedisConnectionManager>();
        services.AddScoped(typeof(ICacheService<>), typeof(CacheService<>));
        return services;
    }
}