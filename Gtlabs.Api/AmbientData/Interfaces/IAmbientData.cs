namespace Gtlabs.Api.AmbientData.Interfaces;

public interface IAmbientData
{
    Guid? GetUserId();
    string GetGatewayUrl();
    Guid? GetCorrelationId();
}