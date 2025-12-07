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
        services.AddSingleton<HeaderAmbientDataProvider>();
        services.AddSingleton<EnvironmentAmbientDataProvider>();
        services.AddSingleton<IAmbientDataProvider>(sp => sp.GetRequiredService<HeaderAmbientDataProvider>());
        services.AddSingleton<IAmbientDataProvider>(sp => sp.GetRequiredService<EnvironmentAmbientDataProvider>());


        //
        // Register each provider as the interfaces it implements
        //
        services.AddSingleton<IGatewayUrlSource>(sp => sp.GetRequiredService<EnvironmentAmbientDataProvider>());
        services.AddSingleton<IAmbientDataProvider>(sp => sp.GetRequiredService<EnvironmentAmbientDataProvider>());
        
        services.AddSingleton<IUserIdSource>(sp => sp.GetRequiredService<HeaderAmbientDataProvider>());
        services.AddSingleton<ICorrelationIdSource>(sp => sp.GetRequiredService<HeaderAmbientDataProvider>());
        
        //
        // Register aggregator
        //
        services.AddSingleton<IAmbientData, AmbientData>();

        return services;
    }
}