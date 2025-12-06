using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Gtlabs.Authentication.Validators;

public interface IAuthorizationValidator
{
    Task Validate(TokenValidatedContext context);
}