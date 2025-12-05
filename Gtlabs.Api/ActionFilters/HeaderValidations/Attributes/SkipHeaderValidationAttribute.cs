namespace Gtlabs.Api.HeaderValidations.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SkipHeaderValidationAttribute : Attribute { }