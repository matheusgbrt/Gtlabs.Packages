using Gtlabs.Api.AmbientData.Extensions;
using Gtlabs.Api.ApiCall.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.AspNet.Extensions;

public static class InitializeService
{
    public static IServiceCollection AddBasicFeatures(this IServiceCollection services)
    {
        services.AddAmbientData();
        services.AddApiClientCallBuilder();
        return services;

    }
}