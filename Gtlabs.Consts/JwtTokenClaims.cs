namespace Gtlabs.Consts;

public static class JwtTokenClaims
{
    public const string Subject = "sub";
    public const string JwtId = "jti";
    public const string IssuedAt = "iat";
    public const string TokenType = "token_type";

    public const string ValueApp = JwtTokenTypes.App;
    public const string ValueApi = JwtTokenTypes.Api;
    public const string ValueUser = JwtTokenTypes.User;
}

public static class JwtTokenTypes
{
    public const string App = "app";
    public const string Api = "api";
    public const string User = "user";

    public static readonly string[] All = [App, Api, User];

    public static bool IsKnown(string tokenType)
        => All.Contains(tokenType, StringComparer.OrdinalIgnoreCase);
}
