using Gtlabs.AmbientData.Interfaces;
using Gtlabs.AmbientData.Providers;
using Gtlabs.AmbientData.Sources;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.AmbientData.Extensions;


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
        services.AddScoped<IGatewayUrlSource>(sp => sp.GetRequiredService<EnvironmentAmbientDataProvider>());
        services.AddScoped<IAmbientDataProvider>(sp => sp.GetRequiredService<EnvironmentAmbientDataProvider>());
        
        services.AddScoped<IUserIdSource>(sp => sp.GetRequiredService<HeaderAmbientDataProvider>());
        services.AddScoped<ICorrelationIdSource>(sp => sp.GetRequiredService<HeaderAmbientDataProvider>());
        
        //
        // Register aggregator
        //
        services.AddScoped<IAmbientData, AmbientData>();

        return services;
    }
}