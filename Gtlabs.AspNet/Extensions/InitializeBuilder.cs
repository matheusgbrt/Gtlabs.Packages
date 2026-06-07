using Gtlabs.Logging.Extensions;
using Microsoft.Extensions.Hosting;

namespace Gtlabs.AspNet.Extensions;

public static class InitializeBuilder
{
    public static IHostBuilder AddBasicFeatures(
        this IHostBuilder hostBuilder,
        string? seqUrl = null)
    {
        hostBuilder.UseSerilog(seqUrl);
        return hostBuilder;
    }
}
