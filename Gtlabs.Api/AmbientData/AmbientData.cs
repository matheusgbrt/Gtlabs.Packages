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
}