namespace Gtlabs.AmbientData.Interfaces;

public interface IAmbientData
{
    Guid? GetUserId();
    string GetGatewayUrl();
    Guid? GetCorrelationId();
    string GetAppId();
    string GetServiceToken();
}