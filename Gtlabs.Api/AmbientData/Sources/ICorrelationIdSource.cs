namespace Gtlabs.Api.AmbientData.Sources;

public interface ICorrelationIdSource
{
    public Guid? GetCorrelationid();   
}