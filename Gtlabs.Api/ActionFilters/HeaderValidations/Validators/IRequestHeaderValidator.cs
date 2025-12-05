using Microsoft.AspNetCore.Http;

namespace Gtlabs.Api.ActionFilters.HeaderValidations.Validators;

public interface IRequestHeaderValidator
{
    string? Validate(HttpContext context);
}