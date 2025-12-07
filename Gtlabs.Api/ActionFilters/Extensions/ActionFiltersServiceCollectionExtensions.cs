using Gtlabs.Api.ActionFilters.HeaderValidations.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Api.ActionFilters.Extensions;

public static class ActionFiltersServiceCollectionExtensions
{
    public static IServiceCollection AddActionFilters(this IServiceCollection services)
    {
        services.AddTransient<IRequestHeaderValidator, UserIdValidator>();

        return services;
    }
}