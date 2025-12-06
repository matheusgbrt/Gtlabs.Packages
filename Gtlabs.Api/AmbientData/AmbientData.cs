using Gtlabs.Api.AmbientData.Interfaces;
using Gtlabs.Api.AmbientData.Sources;

namespace Gtlabs.Api.AmbientData;

public class AmbientData : IAmbientData
{
    private readonly IEnumerable<IAmbientDataProvider> _providers;

    public AmbientData(IEnumerable<IAmbientDataProvider> providers)
    {
        _providers = providers
            .OrderBy(p => (p as IOrderedAmbientSource)?.Order ?? int.MaxValue)
            .ToList();
    }

    
    public Guid? GetUserId()
    {
        foreach (var provider in _providers.OfType<IUserIdSource>())
        {
            var value = provider.GetUserId();
            if (value != null)
                return value;
        }

        return null;
    }

    public string GetGatewayUrl()
    {
        foreach (var provider in _providers.OfType<IGatewayUrlSource>())
        {
            var value = provider.GetGatewayUrl();
            if (!string.IsNullOrWhiteSpace(value))
                return value!;
        }

        throw new InvalidOperationException(
            "Gateway URL not found in any ambient data provider or environment variable.");
    }

    public Guid? GetCorrelationId()
    {
        foreach (var provider in _providers.OfType<ICorrelationIdSource>())
        {
            var value = provider.GetCorrelationid();
            if (value.HasValue)
                return value.Value;
        }

        return null;
    }

    public string GetAppId()
    {
        foreach (var provider in _providers.OfType<IAppIdSource>())
        {
            var value = provider.GetAppId();
            if (!string.IsNullOrEmpty(value))
                return value;
        }

        throw new InvalidOperationException(
            "AppId not found in any ambient data provider or environment variable.");
    }
}