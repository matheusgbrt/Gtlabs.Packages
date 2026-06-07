using Gtlabs.Logging.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Gtlabs.Logging.Extensions;

public static class OpenTelemetryRegistrationExtension
{
    public static IServiceCollection AddGtlabsTracing(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<GtlabsTracingOptions>? configure = null)
    {
        var options = new GtlabsTracingOptions();
        ApplyConfiguration(options, configuration);
        configure?.Invoke(options);

        return services.AddGtlabsTracing(options);
    }

    public static IServiceCollection AddGtlabsTracing(
        this IServiceCollection services,
        Action<GtlabsTracingOptions> configure)
    {
        var options = new GtlabsTracingOptions();
        configure(options);

        return services.AddGtlabsTracing(options);
    }

    private static IServiceCollection AddGtlabsTracing(
        this IServiceCollection services,
        GtlabsTracingOptions options)
    {

        if (string.IsNullOrWhiteSpace(options.ServiceName))
        {
            throw new InvalidOperationException("Tracing service name must be configured.");
        }

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(options.ServiceName))
            .WithTracing(builder =>
            {
                builder
                    .AddSource("Gtlabs.ServiceBus")
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                if (options.ExportToOtlp)
                {
                    builder.AddOtlpExporter(exporterOptions =>
                    {
                        if (options.OtlpEndpoint is not null)
                        {
                            exporterOptions.Endpoint = options.OtlpEndpoint;
                        }
                    });
                }
            });

        return services;
    }

    private static void ApplyConfiguration(GtlabsTracingOptions options, IConfiguration configuration)
    {
        options.ServiceName = configuration["OpenTelemetry:ServiceName"]
                              ?? configuration["AppId"]
                              ?? string.Empty;

        var endpoint = configuration["OpenTelemetry:OtlpEndpoint"]
                       ?? configuration["OpenTelemetry:Endpoint"]
                       ?? configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];

        if (Uri.TryCreate(endpoint, UriKind.Absolute, out var uri))
        {
            options.OtlpEndpoint = uri;
        }

        if (bool.TryParse(configuration["OpenTelemetry:ExportToOtlp"], out var exportToOtlp))
        {
            options.ExportToOtlp = exportToOtlp;
        }
    }
}
