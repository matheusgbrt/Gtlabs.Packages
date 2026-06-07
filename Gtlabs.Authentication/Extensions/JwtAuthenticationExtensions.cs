using System.Text;
using Gtlabs.Consts;
using Gtlabs.Consts.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Gtlabs.Authentication.Extensions;

public static class JwtAuthenticationExtensions
{
    private static readonly string[] RequiredClaims =
    [
        JwtTokenClaims.Subject,
        JwtTokenClaims.JwtId,
        JwtTokenClaims.IssuedAt,
        JwtTokenClaims.TokenType
    ];

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
        ValidateJwtConfiguration(jwtEmissionConfiguration);

        jwtOpts.TokenValidationParameters = new TokenValidationParameters
        {
            RequireExpirationTime = true,
            RequireSignedTokens = true,

            ValidateIssuer = true,
            ValidIssuer = jwtEmissionConfiguration.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtEmissionConfiguration.Audience,

            ValidateLifetime = true,
            ClockSkew = opts.ClockSkew,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtEmissionConfiguration.SecretKey)),

            AlgorithmValidator = (algorithm, _, _, _) => opts.IsSigningAlgorithmAllowed(algorithm)
            
        };
        jwtOpts.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var missingClaims = RequiredClaims
                    .Where(claim => context.Principal!.FindFirst(claim) is null)
                    .ToArray();

                if (missingClaims.Length > 0)
                {
                    context.Fail($"Missing required claim(s): {string.Join(", ", missingClaims)}.");
                    return Task.CompletedTask;
                }

                var tokenType = context.Principal!.FindFirst(JwtTokenClaims.TokenType)!.Value;

                if (!JwtTokenTypes.IsKnown(tokenType))
                {
                    context.Fail($"Unknown token type '{tokenType}'.");
                    return Task.CompletedTask;
                }

                if (!opts.IsTokenTypeAllowed(tokenType))
                {
                    context.Fail($"Token type '{tokenType}' is not permitted in this service.");
                }

                return Task.CompletedTask;
            }
        };
    }

    private static void ValidateJwtConfiguration(JwtEmissionConfiguration jwtEmissionConfiguration)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(jwtEmissionConfiguration.Issuer);
        ArgumentException.ThrowIfNullOrWhiteSpace(jwtEmissionConfiguration.Audience);
        ArgumentException.ThrowIfNullOrWhiteSpace(jwtEmissionConfiguration.SecretKey);

        if (Encoding.UTF8.GetByteCount(jwtEmissionConfiguration.SecretKey) < 32)
        {
            throw new InvalidOperationException("Jwt:SecretKey must be at least 32 bytes for HS256.");
        }
    }
}
