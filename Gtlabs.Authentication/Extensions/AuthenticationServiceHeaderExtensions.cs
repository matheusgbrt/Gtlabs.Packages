using Gtlabs.Consts.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Authentication.Extensions;

public static class AuthenticationServiceHeaderExtensions
{
    public static IServiceCollection UseAuthenticationServiceHeader(this IServiceCollection services, Action<AuthenticationHeaderOptions> options)
        => services.AddGtlabsAuthentication(options);

    public static IServiceCollection AddGtlabsAuthentication(this IServiceCollection services, Action<AuthenticationHeaderOptions> options)
    {
        services.Configure(options);
        services.AddJwtAuthentication();
        
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
        return services;
    }
}
