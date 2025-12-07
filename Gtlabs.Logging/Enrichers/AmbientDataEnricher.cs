using Gtlabs.AmbientData.Interfaces;
using Serilog.Core;
using Serilog.Events;

namespace Gtlabs.Logging.Enrichers;

public class AmbientDataEnricher : ILogEventEnricher
{
    private readonly IAmbientData _ambientData;

    public AmbientDataEnricher(IAmbientData ambientData)
    {
        _ambientData = ambientData;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        Guid? correlationId = _ambientData.GetCorrelationId();
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CorrelationId", correlationId));
        Guid? userId = _ambientData.GetUserId();
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserId", userId));
        string serviceName = _ambientData.GetAppId();
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ServiceName", serviceName));
    }
}