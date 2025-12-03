using Consul;
using Gtlabs.Consts;
using Gtlabs.Consul.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Consul.Extensions;


public static class ConsulRegistration
{
    public static IServiceCollection AddConsulRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        var appId = configuration[ConfigurationFields.AppId];
        if (string.IsNullOrWhiteSpace(appId))
            throw new ArgumentException("AppId must be provided.", nameof(appId));

        services.AddSingleton(new ConsulConfig { AppId = appId });

        services.AddSingleton(sp =>
        {
            var addr = Environment.GetEnvironmentVariable("URL-CONSUL");
            if (string.IsNullOrWhiteSpace(addr))
                throw new InvalidOperationException("Providers variable 'URL-CONSUL' is not set.");

            return new ConsulClient(c => c.Address = new Uri(addr));
        });

        services.AddHostedService<ConsulRegistrationHostedService>();
        return services;
    }
}
