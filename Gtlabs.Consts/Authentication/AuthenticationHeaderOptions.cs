namespace Gtlabs.Consts.Authentication;

public class AuthenticationHeaderOptions
{
    public bool UseAuthHeader { get; set; } = true;
    public TimeSpan ClockSkew { get; set; } = TimeSpan.FromMinutes(1);
    public List<string> AllowedSigningAlgorithms { get; set; } = ["HS256"];
    public List<string> AllowedTokenTypes { get; set; } = [JwtTokenTypes.App];

    public void AllowOnly(params string[] tokenTypes)
    {
        AllowedTokenTypes = tokenTypes.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }

    public void AllowAppTokensOnly()
        => AllowOnly(JwtTokenTypes.App);

    public void AllowUserTokensOnly()
        => AllowOnly(JwtTokenTypes.User);

    public void AllowApiTokensOnly()
        => AllowOnly(JwtTokenTypes.Api);

    public void AllowAppAndUserTokens()
        => AllowOnly(JwtTokenTypes.App, JwtTokenTypes.User);

    public void AllowAllTokenTypes()
        => AllowOnly(JwtTokenTypes.App, JwtTokenTypes.Api, JwtTokenTypes.User);

    public bool IsTokenTypeAllowed(string tokenType)
        => AllowedTokenTypes.Contains(tokenType, StringComparer.OrdinalIgnoreCase);

    public bool IsSigningAlgorithmAllowed(string algorithm)
        => AllowedSigningAlgorithms.Contains(algorithm, StringComparer.Ordinal);
}
