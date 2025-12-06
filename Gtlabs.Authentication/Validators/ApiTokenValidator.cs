using Gtlabs.Consts;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Gtlabs.Authentication.Validators;

[TokenType(JwtTokenClaims.ValueApi)]
public class ApiTokenValidator : IAuthorizationValidator
{
    public Task Validate(TokenValidatedContext context)
    {
        return Task.CompletedTask;
    }
}