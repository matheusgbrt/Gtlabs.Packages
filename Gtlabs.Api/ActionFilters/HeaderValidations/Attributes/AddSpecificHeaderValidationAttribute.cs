namespace Gtlabs.Api.ActionFilters.HeaderValidations.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class AddSpecificHeaderValidationAttribute : Attribute
{
    public Type ValidatorType { get; }

    public AddSpecificHeaderValidationAttribute(Type validatorType)
    {
        ValidatorType = validatorType;
    }
}