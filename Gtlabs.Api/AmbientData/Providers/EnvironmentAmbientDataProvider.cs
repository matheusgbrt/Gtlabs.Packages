using Gtlabs.Api.AmbientData.Interfaces;
using Gtlabs.Api.AmbientData.Sources;

namespace Gtlabs.Api.AmbientData.Providers;

public class EnvironmentAmbientDataProvider: IAmbientDataProvider, IGatewayUrlSource, IOrderedAmbientSource
{
    public string GetGatewayUrl()
    {
        var value = System.Environment.GetEnvironmentVariable("GATEWAY_URL");
        if (!string.IsNullOrWhiteSpace(value))
            return value.Trim().TrimEnd('/');
        return string.Empty;
    }

    public int Order { get; } = 1;
}