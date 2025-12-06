using Gtlabs.Authentication.Validators;
using Gtlabs.Consts;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Authentication.Extensions;

public static class AuthorizationValidatorExtensions
{
    public static IServiceCollection AddAuthorizationValidators(this IServiceCollection services)
    {
        var registry = new AuthorizationValidatorRegistry();

        var assemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => a.FullName.StartsWith("Gtlabs"));
        
        var validatorTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                typeof(IAuthorizationValidator).IsAssignableFrom(t) &&
                !t.IsAbstract &&
                t.GetCustomAttributes(typeof(TokenTypeAttribute), false).Any()
            );

        foreach (var validatorType in validatorTypes)
        {
            var attributes = validatorType.GetCustomAttributes(typeof(TokenTypeAttribute), false)
                .Cast<TokenTypeAttribute>();

            foreach (var attr in attributes)
            {
                registry.Register(attr.TokenType, validatorType);
            }

            services.AddScoped(validatorType);
        }

        services.AddSingleton(registry);

        return services;
    }
}