using Gtlabs.AmbientData.Interfaces;
using Gtlabs.Consts;
using Microsoft.AspNetCore.Http;

namespace Gtlabs.Api.ActionFilters.HeaderValidations.Validators;

public class ServiceTokenValidator : IRequestHeaderValidator
{
    private readonly IAmbientData _ambientData;

    public ServiceTokenValidator(IAmbientData ambientData)
    {
        _ambientData = ambientData;
    }

    public string? Validate(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(HeaderFields.ServiceKey, out var key))
            return $"Missing {HeaderFields.ServiceKey}";
        
        var token = context.Request.Headers[HeaderFields.ServiceKey].First()!;
        
        if (token != _ambientData.GetServiceToken())
            return ("Invalid token");
        return null;
    }
}