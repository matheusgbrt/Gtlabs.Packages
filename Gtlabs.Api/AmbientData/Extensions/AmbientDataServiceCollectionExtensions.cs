using Gtlabs.Api.AmbientData.Interfaces;
using Gtlabs.Api.AmbientData.Providers;
using Gtlabs.Api.AmbientData.Sources;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Api.AmbientData.Extensions;


public static class AmbientDataServiceCollectionExtensions
{
    public static IServiceCollection AddAmbientData(this IServiceCollection services)
    {
        //
        // Register providers here manually
        //
        services.AddScoped<HeaderAmbientDataProvider>();
        services.AddScoped<EnvironmentAmbientDataProvider>();
        services.AddScoped<IAmbientDataProvider>(sp => sp.GetRequiredService<HeaderAmbientDataProvider>());
        services.AddScoped<IAmbientDataProvider>(sp => sp.GetRequiredService<EnvironmentAmbientDataProvider>());


        //
        // Register each provider as the interfaces it implements
        //
        services.AddScoped<IUserIdSource>(sp => sp.GetRequiredService<HeaderAmbientDataProvider>());
        services.AddScoped<IGatewayUrlSource>(sp => sp.GetRequiredService<EnvironmentAmbientDataProvider>());
        
        //
        // Register aggregator
        //
        services.AddScoped<IAmbientData, AmbientData>();

        return services;
    }
}