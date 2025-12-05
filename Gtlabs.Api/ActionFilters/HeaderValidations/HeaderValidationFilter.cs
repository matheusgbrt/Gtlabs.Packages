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

        if (metadata.OfType<SkipHeaderValidationAttribute>().Any())
            return;

        var skipAttributes = metadata.OfType<SkipSpecificHeaderValidationAttribute>();
        var skipSet = new HashSet<Type>(skipAttributes.Select(a => a.ValidatorType));

        var validatorsToRun = _validators
            .Where(v => !skipSet.Contains(v.GetType()));

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