namespace Gtlabs.Core.AmbientData.Sources;

public interface ICorrelationIdSource
{
    public Guid? GetCorrelationid();   
}