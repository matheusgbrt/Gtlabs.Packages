using Gtlabs.Api.AmbientData.Interfaces;
using Gtlabs.Api.AmbientData.Providers;
using Gtlabs.Api.AmbientData.Sources;
using Gtlabs.Api.ApiCall.Normalization;
using Gtlabs.Api.ApiCall.Normalization.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Api.ApiCall.Extensions;

public static class HeaderNormalizationServiceCollectionExtensions
{
    public static IServiceCollection AddApiClientCallBuilder(this IServiceCollection services)
    {

        services.AddSingleton<IHeaderNormalizationProvider, CorrelationIdHeaderNormalizer>();
        services.AddSingleton<IHeaderNormalizationProvider, UserIdHeaderNormalizer>();
        services.AddSingleton<IHeaderNormalizationProvider, ServiceNameNormalizer>();
        services.AddSingleton<IHeaderNormalizationProvider, TimestampHeaderNormalizer>();


        services.AddSingleton<ApiHeaderNormalizationService>();
        services.AddTransient<IApiClientCall, ApiClientCall>();
        services.AddTransient<IApiClientCallBuilder, ApiClientCallBuilder>();
        
        return services;
    }
}