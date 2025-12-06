using Gtlabs.Api.ApiCall.Normalization;
using Gtlabs.Api.ApiCall.Normalization.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Api.ApiCall.Extensions;

public static class HeaderNormalizationServiceCollectionExtensions
{
    public static IServiceCollection AddApiClientCallBuilder(this IServiceCollection services)
    {

        services.AddScoped<IHeaderNormalizationProvider, CorrelationIdHeaderNormalizer>();
        services.AddScoped<IHeaderNormalizationProvider, UserIdHeaderNormalizer>();
        services.AddScoped<IHeaderNormalizationProvider, ServiceNameNormalizer>();
        services.AddScoped<IHeaderNormalizationProvider, TimestampHeaderNormalizer>();
        services.AddScoped<IHeaderNormalizationProvider, AppJwtHeaderNormalizer>();



        services.AddScoped<ApiHeaderNormalizationService>();
        services.AddTransient<IApiClientCall, ApiClientCall>();
        services.AddTransient<IApiClientCallBuilder, ApiClientCallBuilder>();
        
        return services;
    }
}