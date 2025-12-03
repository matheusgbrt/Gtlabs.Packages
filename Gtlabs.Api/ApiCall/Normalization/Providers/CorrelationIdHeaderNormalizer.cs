using Gtlabs.Api.AmbientData;
using Gtlabs.Api.AmbientData.Interfaces;
using Gtlabs.Consts;

namespace Gtlabs.Api.ApiCall.Normalization.Providers;

public class CorrelationIdHeaderNormalizer : IHeaderNormalizationProvider
{
    public int Order => 1;
    private readonly IAmbientData _ambientData;

    public CorrelationIdHeaderNormalizer(IAmbientData ambientData)
    {
        _ambientData = ambientData;
    }

    public void Normalize(ApiClientCallPrototype prototype)
    {
        if (!prototype.Headers.ContainsKey(HeaderFields.CorrelationId))
        {
            var correlationId = _ambientData.GetCorrelationId();
            if (correlationId.HasValue)
            {
                prototype.Headers[HeaderFields.CorrelationId] = correlationId.Value.ToString();
            }
            else
            {
                prototype.Headers[HeaderFields.CorrelationId] = Guid.NewGuid().ToString();
            }

        }
    }
}