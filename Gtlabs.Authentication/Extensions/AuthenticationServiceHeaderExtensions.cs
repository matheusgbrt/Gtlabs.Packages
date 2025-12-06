using Gtlabs.Authentication.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Authentication.Extensions;

public static class AuthenticationServiceHeaderExtensions
{
    public static IServiceCollection UseAuthenticationServiceHeader(this IServiceCollection services, Action<AuthenticationHeaderOptions> options)
    {
        services.Configure(options);
        services.AddAuthorizationValidators();
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