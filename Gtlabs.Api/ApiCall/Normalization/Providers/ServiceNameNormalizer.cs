using Gtlabs.Consts;

namespace Gtlabs.Api.ApiCall.Normalization.Providers;

public class ServiceNameNormalizer : IHeaderNormalizationProvider
{
    public int Order => 3;

    public async Task Normalize(ApiClientCallPrototype prototype)
    {
        if (!string.IsNullOrWhiteSpace(prototype.ServiceName))
            prototype.Headers[HeaderFields.ServiceName] = prototype.ServiceName;
        await Task.CompletedTask;
    }
}