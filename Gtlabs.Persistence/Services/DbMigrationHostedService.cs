using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gtlabs.Persistence.Services;


public class DbMigrationHostedService<TContext> : IHostedService
    where TContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DbMigrationHostedService<TContext>> _logger;

    public DbMigrationHostedService(IServiceProvider serviceProvider, ILogger<DbMigrationHostedService<TContext>> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

            var pending = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken);
            if (pending.Any())
            {
                _logger.LogInformation("Applying {Count} pending migrations for {DbContext}.", pending.Count(), typeof(TContext).Name);
                await dbContext.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Migrations applied successfully for {DbContext}.", typeof(TContext).Name);
            }
            else
            {
                _logger.LogInformation("No pending migrations for {DbContext}.", typeof(TContext).Name);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error applying migrations for {DbContext}.", typeof(TContext).Name);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}