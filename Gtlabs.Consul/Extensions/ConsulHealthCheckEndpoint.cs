using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Gtlabs.Consul.Extensions;

public static class ConsulHealthCheckEndpoint
{
    public static WebApplication AddConsulHealthCheck(this WebApplication app)
    {
        app.MapGet("/health", async (HttpContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ConsulHealthCheck");
            logger.LogDebug("Health check invoked from {RemoteIp}", context.Connection.RemoteIpAddress);
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("Healthy");
        });

        return app;
    }
}
