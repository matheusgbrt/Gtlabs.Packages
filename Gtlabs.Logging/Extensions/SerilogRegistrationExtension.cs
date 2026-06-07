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
        this IHostBuilder hostBuilder,
        string? seqUrl = null)
    {
        return hostBuilder.UseSerilog((context, services, configuration) =>
        {
            var resolvedSeqUrl = seqUrl
                                 ?? context.Configuration["Seq:Url"]
                                 ?? context.Configuration["Serilog:SeqUrl"];

            var loggerConfiguration = configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.With(new AmbientDataEnricher(services.GetRequiredService<IAmbientData>()))
                .WriteTo.Console(new RenderedCompactJsonFormatter());

            if (!string.IsNullOrWhiteSpace(resolvedSeqUrl))
            {
                loggerConfiguration.WriteTo.Seq(resolvedSeqUrl);
            }
        });
    }
}
