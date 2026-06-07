using Gtlabs.Api.ApiCall.Authentication;
using Gtlabs.Api.ApiCall.Normalization;
using Gtlabs.Api.ApiCall.Normalization.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Api.ApiCall.Extensions;

public static class HeaderNormalizationServiceCollectionExtensions
{
    private const string AuthorizationServiceName = "authentication";
    private const string TokenEndpoint = "api/authentication/app-token";

    public static IServiceCollection AddApiClientCallBuilder(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.Configure<AppTokenHydrationOptions>(options =>
        {
            options.AuthorizationServiceName = AuthorizationServiceName;
            options.TokenEndpoint = TokenEndpoint;
        });
        services.AddHttpClient<IAppTokenProvider, CachedAppTokenProvider>();

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
