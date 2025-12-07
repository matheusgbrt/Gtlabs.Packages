namespace Gtlabs.Api.ApiCall.Normalization;

public interface IHeaderNormalizationProvider : IOrderedHeaderNormalizer
{
    Task Normalize(ApiClientCallPrototype prototype);
}