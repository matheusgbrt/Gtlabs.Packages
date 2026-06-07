# Gtlabs.Logging

Serilog setup and enrichers for GT Labs services.

Provides logging registration helpers and ambient-data enrichment for service name, correlation id, and user id.

## Serilog

Register Serilog and optionally configure Seq:

```csharp
builder.Host.UseSerilog("https://seq.example.com");
```

If no URL is passed, the extension reads `Seq:Url` or `Serilog:SeqUrl` from configuration. If neither is configured, logs are written only to console.

## Distributed Tracing

Register OpenTelemetry tracing:

```csharp
services.AddGtlabsTracing(configuration);
```

By default, tracing reads:

- `OpenTelemetry:ServiceName`, falling back to `AppId`
- `OpenTelemetry:OtlpEndpoint`, falling back to `OpenTelemetry:Endpoint` or `OTEL_EXPORTER_OTLP_ENDPOINT`
- `OpenTelemetry:ExportToOtlp`

Example configuration:

```json
{
  "AppId": "orders",
  "OpenTelemetry": {
    "OtlpEndpoint": "http://otel-collector:4317",
    "ExportToOtlp": true
  }
}
```

You can still override values in code:

```csharp
services.AddGtlabsTracing(configuration, options =>
{
    options.ServiceName = "orders-worker";
});
```

The package instruments ASP.NET Core inbound requests, outgoing `HttpClient` calls, and the `Gtlabs.ServiceBus` activity source used by Rebus tracing. Logs are enriched with `TraceId`, `SpanId`, and `ParentSpanId` when an activity is active.
