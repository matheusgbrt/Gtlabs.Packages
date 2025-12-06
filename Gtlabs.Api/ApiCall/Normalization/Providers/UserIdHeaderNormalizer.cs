using Gtlabs.AmbientData.Interfaces;
using Gtlabs.Consts;

namespace Gtlabs.Api.ApiCall.Normalization.Providers;

public class UserIdHeaderNormalizer : IHeaderNormalizationProvider
{
    public int Order => 2;

    private readonly IAmbientData _ambient;

    public UserIdHeaderNormalizer(IAmbientData ambient)
    {
        _ambient = ambient;
    }
    
    public void Normalize(ApiClientCallPrototype prototype)
    {
        var userId = _ambient.GetUserId();
        
        if (userId != null)
            prototype.Headers[HeaderFields.UserId] = userId.Value.ToString();
    }
}