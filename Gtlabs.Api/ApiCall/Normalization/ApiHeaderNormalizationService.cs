namespace Gtlabs.Api.ApiCall.Normalization;

public class ApiHeaderNormalizationService
{
    private readonly IEnumerable<IHeaderNormalizationProvider> _providers;

    public ApiHeaderNormalizationService(IEnumerable<IHeaderNormalizationProvider> providers)
    {
        _providers = providers
            .OrderBy(p => p.Order)
            .ToList();
    }

    public void Apply(ApiClientCallPrototype prototype)
    {
        foreach (var provider in _providers)
            provider.Normalize(prototype);
    }
}