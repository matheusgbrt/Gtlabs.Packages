using Gtlabs.Core.AmbientData.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gtlabs.Persistence.CustomDbContext;

public class GtLabsDbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext>
    where TContext : GtLabsDbContext
{
    public TContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TContext>();

        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

        optionsBuilder.UseNpgsql(connectionString);

        IAmbientData ambientData = new DesignTimeAmbientData();
        return (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options, ambientData)!;
    }

    private class DesignTimeAmbientData : IAmbientData
    {
        public Guid? GetUserId() => Guid.Empty;
        public string GetGatewayUrl() => string.Empty;
        public Guid? GetCorrelationId() => Guid.Empty;
        public string GetAppId() => string.Empty;
    }
}