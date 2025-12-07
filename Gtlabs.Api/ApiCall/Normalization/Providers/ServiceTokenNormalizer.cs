using Gtlabs.AmbientData.Interfaces;
using Gtlabs.Consts;

namespace Gtlabs.Api.ApiCall.Normalization.Providers;

public class ServiceTokenNormalizer : IHeaderNormalizationProvider
{
    public int Order => 6;
    private readonly IAmbientData _ambientData;

    public ServiceTokenNormalizer(IAmbientData ambientData)
    {
        _ambientData = ambientData;
    }

    public async Task Normalize(ApiClientCallPrototype prototype)
    {
        if (prototype.UseAppToken)
            prototype.Headers[HeaderFields.ServiceKey] = _ambientData.GetServiceToken();
        await Task.CompletedTask;
    }
}