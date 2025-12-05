using Gtlabs.Api.ActionFilters.HeaderValidations;
using Gtlabs.Api.AmbientData.Extensions;
using Gtlabs.Api.ApiCall.Extensions;
using Gtlabs.DependencyInjections.DependencyInjectons.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.AspNet.Extensions;

public static class InitializeService
{
    public static IServiceCollection AddBasicFeatures(this IServiceCollection services)
    {
        services.AddAmbientData();
        services.AddApiClientCallBuilder();
        services.RegisterAllDependencies();
        services.AddControllers(options =>
        {
            options.Filters.Add<HeaderValidationFilter>();
        });
        return services;

    }
}