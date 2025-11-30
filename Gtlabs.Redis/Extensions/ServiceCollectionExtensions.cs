using Gtlabs.Redis.Interfaces;
using Gtlabs.Redis.Options;
using Gtlabs.Redis.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Redis.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisOptions>(configuration.GetSection("Redis"));
        services.AddSingleton<RedisConnectionManager>();
        services.AddScoped(typeof(ICacheService<>), typeof(CacheService<>));
        return services;
    }
}