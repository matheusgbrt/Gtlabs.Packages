using Gtlabs.AmbientData.Interfaces;
using Gtlabs.AmbientData.Sources;
using Gtlabs.Consts;
using Microsoft.Extensions.Configuration;

namespace Gtlabs.AmbientData.Providers;

public class EnvironmentAmbientDataProvider: IAmbientDataProvider, IGatewayUrlSource, IAppIdSource,IOrderedAmbientSource
{
    
    private readonly IConfiguration _config;

    public EnvironmentAmbientDataProvider(IConfiguration config)
    {
        _config = config;
    }

    public string GetGatewayUrl()
    {
        var value = Environment.GetEnvironmentVariable(ConfigurationFields.GatewayUrl);
        if (!string.IsNullOrWhiteSpace(value))
            return value.Trim().TrimEnd('/');
        return string.Empty;
    }
    
    public string GetAppId()
    {
        return _config[ConfigurationFields.AppId]!;
    }

    public int Order { get; } = 1;
}