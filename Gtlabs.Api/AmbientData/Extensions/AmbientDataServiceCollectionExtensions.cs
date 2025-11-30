using Gtlabs.Api.AmbientData.Headers;
using Gtlabs.Api.AmbientData.Sources;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Api.AmbientData;


public static class AmbientDataServiceCollectionExtensions
{
    public static IServiceCollection AddAmbientData(this IServiceCollection services)
    {
        //
        // Register providers here manually
        //
        services.AddScoped<IAmbientDataProvider>(sp => sp.GetRequiredService<HeaderAmbientDataProvider>());


        //
        // Register each provider as the interfaces it implements
        //
        services.AddScoped<IUserIdSource>(sp => sp.GetRequiredService<HeaderAmbientDataProvider>());
        
        //
        // Register aggregator
        //
        services.AddScoped<IAmbientData, AmbientData>();

        return services;
    }
}