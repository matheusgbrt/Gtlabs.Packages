namespace Gtlabs.AmbientData.Sources;

public interface ICorrelationIdSource
{
    public Guid? GetCorrelationid();   
}