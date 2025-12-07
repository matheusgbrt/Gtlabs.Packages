using Gtlabs.Api.ActionFilters.HeaderValidations.Attributes;
using Gtlabs.Api.ActionFilters.HeaderValidations.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gtlabs.Api.ActionFilters.HeaderValidations;

public class HeaderValidationFilter : IActionFilter
{
    private readonly IEnumerable<IRequestHeaderValidator> _validators;
    private readonly IServiceProvider _serviceProvider;

    public HeaderValidationFilter(IServiceProvider serviceProvider,
        IEnumerable<IRequestHeaderValidator> validators)
    {
        _serviceProvider = serviceProvider;
        _validators = validators;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var metadata = context.ActionDescriptor.EndpointMetadata;

        // Skip everything attribute
        if (metadata.OfType<SkipHeaderValidationAttribute>().Any())
            return;

        // Collect types to skip
        var skipTypes = metadata
            .OfType<SkipSpecificHeaderValidationAttribute>()
            .Select(a => a.ValidatorType)
            .ToHashSet();

        // Always skip ServiceTokenValidator unless explicitly re-added
        skipTypes.Add(typeof(ServiceTokenValidator));

        // Collect types to explicitly add back
        var addTypes = metadata
            .OfType<AddSpecificHeaderValidationAttribute>()
            .Select(a => a.ValidatorType)
            .ToHashSet();

        // Base = all validators minus skipped ones
        var validatorsToRun = _validators
            .Where(v => !skipTypes.Contains(v.GetType()))
            .ToList();

        // Add back validators by TYPE (not new instantiation — reuse from existing list)
        foreach (var type in addTypes)
        {
            var validator = _validators.FirstOrDefault(v => v.GetType() == type);
            if (validator != null && !validatorsToRun.Contains(validator))
            {
                validatorsToRun.Add(validator);
            }
        }

        // Execute validators
        foreach (var validator in validatorsToRun)
        {
            var error = validator.Validate(context.HttpContext);
            if (error != null)
            {
                context.Result = new BadRequestObjectResult(error);
                return;
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}