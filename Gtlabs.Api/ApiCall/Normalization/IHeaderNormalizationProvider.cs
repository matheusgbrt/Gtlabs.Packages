namespace Gtlabs.Api.ApiCall.Normalization;

public interface IHeaderNormalizationProvider : IOrderedHeaderNormalizer
{
    void Normalize(ApiClientCallPrototype prototype);
}