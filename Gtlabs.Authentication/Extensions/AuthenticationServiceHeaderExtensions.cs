using Gtlabs.Authentication.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Authentication.Extensions;

public static class AuthenticationServiceHeaderExtensions
{
    public static IServiceCollection UseAuthenticationServiceHeader(this IServiceCollection services, Action<AuthenticationHeaderOptions> options)
    {
        services.Configure(options);
        services.AddAuthorizationValidators();
        services.AddJwtAuthentication();
        return services;
    }
}