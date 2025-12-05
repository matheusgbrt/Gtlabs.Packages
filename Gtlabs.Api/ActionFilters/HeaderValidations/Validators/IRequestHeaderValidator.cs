using Microsoft.AspNetCore.Http;

namespace Gtlabs.Api.HeaderValidations;

public interface IRequestHeaderValidator
{
    string? Validate(HttpContext context);
}