namespace Gtlabs.Api.ActionFilters.HeaderValidations.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class SkipSpecificHeaderValidationAttribute : Attribute
{
    public Type ValidatorType { get; }

    public SkipSpecificHeaderValidationAttribute(Type validatorType)
    {
        ValidatorType = validatorType;
    }
}