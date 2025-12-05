using Gtlabs.Consts;
using Microsoft.AspNetCore.Http;

namespace Gtlabs.Api.ActionFilters.HeaderValidations.Validators;

public class UserIdValidator : IRequestHeaderValidator
{
    public string? Validate(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(HeaderFields.UserId, out var key))
            return $"Missing {HeaderFields.UserId}";
        return null;
    }
}