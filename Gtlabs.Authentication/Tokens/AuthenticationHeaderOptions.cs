namespace Gtlabs.Authentication.Tokens;

public class AuthenticationHeaderOptions
{
    public bool UseAuthHeader { get; set; } = true;
    public bool AllowOutsideCalls { get; set; } = false;
}