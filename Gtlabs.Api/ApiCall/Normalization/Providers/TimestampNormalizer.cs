using Gtlabs.Consts;

namespace Gtlabs.Api.ApiCall.Normalization.Providers;

public class TimestampHeaderNormalizer : IHeaderNormalizationProvider
{
    public int Order => 4;

    public void Normalize(ApiClientCallPrototype prototype)
    {
        prototype.Headers[HeaderFields.Timestamp] = DateTime.UtcNow.ToString("o");
    }
}