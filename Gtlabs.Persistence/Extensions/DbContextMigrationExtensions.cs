using Microsoft.EntityFrameworkCore;

namespace Gtlabs.Persistence.Extensions;

public static class DbContextMigrationExtensions
{
    public static void ApplyPendingMigrations<T>(this T context)
        where T : DbContext
    {
        if (context.Database.GetPendingMigrations().Any())
            context.Database.Migrate();
    }
}