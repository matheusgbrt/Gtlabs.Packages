using Gtlabs.AmbientData.Interfaces;
using Gtlabs.Logging.Enrichers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;

namespace Gtlabs.Logging.Extensions;

public static class SerilogRegistrationExtension
{
    public static IHostBuilder UseSerilog(
        this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.With(new AmbientDataEnricher(services.GetRequiredService<IAmbientData>()))
                .WriteTo.Console(new RenderedCompactJsonFormatter())
                .WriteTo.Seq("https://seq.gtlabs.com.br");
        });
    }
}