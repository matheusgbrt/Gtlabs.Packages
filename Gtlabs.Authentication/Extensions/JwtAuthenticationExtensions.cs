using System.Text;
using Gtlabs.Authentication.Tokens;
using Gtlabs.Authentication.Validators;
using Gtlabs.Consts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Gtlabs.Authentication.Extensions;

public static class JwtAuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        services.AddSingleton<IConfigureOptions<JwtBearerOptions>>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<AuthenticationHeaderOptions>>().Value;
            var jwtConfig = sp.GetRequiredService<IOptions<JwtEmissionConfiguration>>().Value;
            
            return new ConfigureNamedOptions<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                jwtOpts =>
                {
                    if (!opts.UseAuthHeader)
                        return;

                    ConfigureJwtBearerOptions(jwtOpts, opts,jwtConfig);
                });
        });

        return services;
    }
    
    private static void ConfigureJwtBearerOptions(
        JwtBearerOptions jwtOpts,
        AuthenticationHeaderOptions opts,
        JwtEmissionConfiguration jwtEmissionConfiguration)
    {
        jwtOpts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtEmissionConfiguration.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtEmissionConfiguration.Audience,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtEmissionConfiguration.SecretKey))
            
        };
        jwtOpts.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var registry = context.HttpContext.RequestServices.GetRequiredService<AuthorizationValidatorRegistry>();

                var tokenType = context.Principal!.FindFirst(JwtTokenClaims.TokenType)?.Value;

                if (tokenType == null)
                {
                    context.Fail("Missing token type.");
                }

                if (!opts.AllowOutsideCalls && tokenType != JwtTokenClaims.ValueApp)
                {
                    context.Fail("Token not permitted in this app.");
                }
                
                var validatorType = registry.GetValidatorType(tokenType!);

                if (validatorType == null)
                {
                    context.Fail("Internal failed. Validator not registered.");
                }

                var validator = (IAuthorizationValidator)
                    context.HttpContext.RequestServices.GetRequiredService(validatorType!);

                await validator.Validate(context);
            }
        };
    }
}