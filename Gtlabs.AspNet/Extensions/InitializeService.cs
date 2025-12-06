using Gtlabs.AmbientData.Extensions;
using Gtlabs.Api.ActionFilters.Extensions;
using Gtlabs.Api.ActionFilters.HeaderValidations;
using Gtlabs.Api.ApiCall.Extensions;
using Gtlabs.Consts;
using Gtlabs.DependencyInjections.DependencyInjectons.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.AspNet.Extensions;

public static class InitializeService
{
    public static IServiceCollection AddBasicFeatures(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddAmbientData();
        services.AddApiClientCallBuilder();
        services.RegisterAllDependencies();
        services.AddActionFilters();
        services.Configure(new Action<JwtEmissionConfiguration>(opt =>
        {
            opt.Issuer = configuration["Jwt:Issuer"]!;
            opt.Audience = configuration["Jwt:Audience"]!;
            opt.SecretKey = configuration["Jwt:SecretKey"]!;
        }));
        services.AddControllers(options =>
        {
            options.Filters.Add<HeaderValidationFilter>();
        });
        return services;

    }
}