using System.Diagnostics;
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

        var activity = Activity.Current;
        if (activity is not null)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", activity.TraceId.ToString()));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SpanId", activity.SpanId.ToString()));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ParentSpanId", activity.ParentSpanId.ToString()));
        }
    }
}
