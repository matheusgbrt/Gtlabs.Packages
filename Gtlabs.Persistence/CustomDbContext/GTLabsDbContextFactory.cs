using Gtlabs.Persistence.DataFilters;
using Gtlabs.Persistence.Entities;
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

        var softDeleteFilter = new DataFilter<ISoftDelete>();

        try
        {
            return (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options, softDeleteFilter)!;
        }
        catch (MissingMethodException)
        {
            return (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options)!;
        }
    }
}
