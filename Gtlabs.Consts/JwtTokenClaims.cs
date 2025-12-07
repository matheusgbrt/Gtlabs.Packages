namespace Gtlabs.Consts;

public class JwtTokenClaims
{
    // Claim Fields
    public static readonly string App = "app";
    public static string PermittedApps = "permitted_apps";
    public static string TokenType = "token_type";
    
    // Claim Values
    public const string ValueApp = "app";
    public const string ValueApi = "api";
    public const string ValueUser = "user";
}