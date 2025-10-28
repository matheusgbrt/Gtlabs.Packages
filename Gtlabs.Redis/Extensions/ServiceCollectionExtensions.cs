using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Redis.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedisCache(this IServiceCollection services)
    {
        services.AddSingleton<RedisConnectionManager>();
        return services;
    }
}