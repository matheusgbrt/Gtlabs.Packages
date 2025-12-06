namespace Gtlabs.Core.AmbientData.Interfaces;

public interface IAmbientData
{
    Guid? GetUserId();
    string GetGatewayUrl();
    Guid? GetCorrelationId();
    string GetAppId();
}