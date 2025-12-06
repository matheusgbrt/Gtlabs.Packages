using Gtlabs.Consts;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Gtlabs.Authentication.Validators;

[TokenType(JwtTokenClaims.ValueUser)]
public class UserTokenValidator : IAuthorizationValidator
{
    public Task Validate(TokenValidatedContext context)
    {
        return Task.CompletedTask;
    }
}