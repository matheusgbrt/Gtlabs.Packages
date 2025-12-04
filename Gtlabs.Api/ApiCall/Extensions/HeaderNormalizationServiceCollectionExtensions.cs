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

        services.AddScoped<IHeaderNormalizationProvider, CorrelationIdHeaderNormalizer>();
        services.AddScoped<IHeaderNormalizationProvider, UserIdHeaderNormalizer>();
        services.AddScoped<IHeaderNormalizationProvider, ServiceNameNormalizer>();
        services.AddScoped<IHeaderNormalizationProvider, TimestampHeaderNormalizer>();


        services.AddScoped<ApiHeaderNormalizationService>();
        services.AddTransient<IApiClientCall, ApiClientCall>();
        services.AddTransient<IApiClientCallBuilder, ApiClientCallBuilder>();
        
        return services;
    }
}