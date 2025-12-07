using Gtlabs.Api.ApiCall.Normalization;
using Gtlabs.Api.ApiCall.Normalization.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Api.ApiCall.Extensions;

public static class HeaderNormalizationServiceCollectionExtensions
{
    public static IServiceCollection AddApiClientCallBuilder(this IServiceCollection services)
    {

        services.AddTransient<IHeaderNormalizationProvider, CorrelationIdHeaderNormalizer>();
        services.AddTransient<IHeaderNormalizationProvider, UserIdHeaderNormalizer>();
        services.AddTransient<IHeaderNormalizationProvider, ServiceNameNormalizer>();
        services.AddTransient<IHeaderNormalizationProvider, TimestampHeaderNormalizer>();
        services.AddTransient<IHeaderNormalizationProvider, AppJwtHeaderNormalizer>();



        services.AddTransient<ApiHeaderNormalizationService>();
        services.AddTransient<IApiClientCall, ApiClientCall>();
        services.AddTransient<IApiClientCallBuilder, ApiClientCallBuilder>();
        
        return services;
    }
}