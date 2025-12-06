namespace Gtlabs.Authentication;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class TokenTypeAttribute : Attribute
{
    public string TokenType { get; }

    public TokenTypeAttribute(string tokenType)
    {
        TokenType = tokenType;
    }
}