using Gtlabs.Core.AmbientData.Interfaces;
using Gtlabs.Persistence.CustomDbContext;
using Gtlabs.Persistence.Repository;
using Gtlabs.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Persistence.Extensions;

public static class AddPersistenceExtension
{
    public static IServiceCollection AddPersistence<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionStringName = "DefaultConnection")
        where TContext : GtLabsDbContext
    {
        var connectionString = configuration.GetConnectionString(connectionStringName)
                               ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");

        services.AddDbContext<TContext>((sp, options) =>
        {
            var ambientData = sp.GetRequiredService<IAmbientData>();
            options.UseNpgsql(connectionString);
            
        });
        
        services.AddHostedService<DbMigrationHostedService<TContext>>();
        services.AddScoped<DbContext, TContext>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        return services;
    }
}