using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gtlabs.Consts;
using Gtlabs.Consts.Authentication;
using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gtlabs.Authentication.Services;

public class JwtEmissorService : IJwtEmissorService, ITransientDependency
{
    private readonly IOptions<JwtEmissionConfiguration> _jwtEmissionConfiguration;

    public JwtEmissorService(IOptions<JwtEmissionConfiguration> jwtEmissionConfiguration)
    {
        _jwtEmissionConfiguration = jwtEmissionConfiguration;
    }

    public string GenerateAppPermissionToken(AppPermission appPermission)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtEmissionConfiguration.Value.SecretKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var now = DateTimeOffset.UtcNow;
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, appPermission.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat,
                now.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64
            ),            new Claim(JwtTokenClaims.App, appPermission.Identifier),
            new Claim(JwtTokenClaims.TokenType,JwtTokenClaims.ValueApp)
        };
        foreach (var app in appPermission.PermittedApps)
        {
            claims.Add(new Claim(JwtTokenClaims.PermittedApps, app.Identifier));
        }
        
        var token = new JwtSecurityToken(
            issuer:_jwtEmissionConfiguration.Value.Issuer,
            audience:_jwtEmissionConfiguration.Value.Audience,
            claims:claims,
            expires: now.UtcDateTime.AddHours(1),
            signingCredentials:signingCredentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}