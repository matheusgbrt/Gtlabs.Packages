using Gtlabs.AmbientData.Interfaces;
using Gtlabs.Consts;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Gtlabs.Authentication.Validators;

[TokenType(JwtTokenClaims.ValueApp)]
public class AppTokenValidator : IAuthorizationValidator
{
    private readonly IAmbientData _ambientData;

    public AppTokenValidator(IAmbientData ambientData)
    {
        _ambientData = ambientData;
    }

    public Task Validate(TokenValidatedContext context)
    {
        var permittedApps = context.Principal!.FindAll(JwtTokenClaims.PermittedApps);
        var claimValues = permittedApps.Select(claim => claim.Value).ToList();
        if (!claimValues.Contains(_ambientData.GetAppId()))
        {
            context.Fail("App not listed in the permission list.");
        }
        return Task.CompletedTask;
    }
}