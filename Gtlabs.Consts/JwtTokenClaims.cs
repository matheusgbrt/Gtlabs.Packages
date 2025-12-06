namespace Gtlabs.Consts;

public class JwtTokenClaims
{
    // Claim Fields
    public static readonly string App = "app";
    public static string PermittedApps = "permitted_apps";
    public static string TokenType = "token_type";
    public static string TokenTypeApi = "token_type_api";
    public static string TokenTypeApp = "token_type_app";
    public static string TokenTypeUser = "token_type_user";
    
    // Claim Values
    public const string ValueApp = "app";
    public const string ValueApi = "api";
    public const string ValueUser = "user";
}