using System;
using System.Runtime.CompilerServices;
using Gtlabs.Api.ApiCall.Normalization;
using Gtlabs.Authentication.Providers;
using Gtlabs.Authentication.Tokens;
using Gtlabs.Authentication.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Authentication.Extensions;

public static class AuthenticationServiceHeaderExtensions
{
    public static IServiceCollection UseAuthenticationServiceHeader(this IServiceCollection services, Action<AuthenticationHeaderOptions> options)
    {
        services.Configure(options);
        services.AddScoped<IHeaderNormalizationProvider, AppJwtHeaderNormalizer>();
        services.AddAuthorizationValidators();
        services.AddJwtAuthentication();
        return services;
    }
}