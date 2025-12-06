namespace Gtlabs.Authentication.Validators;

public class AuthorizationValidatorRegistry
{
    private readonly Dictionary<string, Type> _validators = new();

    public void Register(string tokenType, Type validatorType)
    {
        _validators[tokenType] = validatorType;
    }

    public Type? GetValidatorType(string tokenType)
    {
        return _validators.TryGetValue(tokenType, out var type) 
            ? type 
            : null;
    }
}