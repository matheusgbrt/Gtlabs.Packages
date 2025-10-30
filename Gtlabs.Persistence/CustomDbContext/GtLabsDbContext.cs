using System.Linq.Expressions;
using Gtlabs.Api.AmbientData;
using Gtlabs.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gtlabs.Persistence.CustomDbContext;

public abstract class GtLabsDbContext : DbContext
{
    private readonly IAmbientData _ambientData;
    protected GtLabsDbContext(DbContextOptions options, IAmbientData ambientData) : base(options)
    {
        _ambientData = ambientData;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ApplySoftDeleteGlobalFilter(modelBuilder);
    }

    private static void ApplySoftDeleteGlobalFilter(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(SoftDeleteEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var isDeletedProperty = Expression.PropertyOrField(parameter, nameof(SoftDeleteEntity.IsDeleted));
                var compareExpression = Expression.Equal(isDeletedProperty, Expression.Constant(false));
                var lambda = Expression.Lambda(compareExpression, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = _ambientData?.GetUserId();
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries<AuditedEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreationTime = now;
                    entry.Entity.CreatorId = userId;
                    entry.Entity.LastModificationTime = now;
                    entry.Entity.ModifierId = userId;
                    entry.Entity.IsDeleted = false;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModificationTime = now;
                    entry.Entity.ModifierId = userId;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.LastModificationTime = now;
                    entry.Entity.ModifierId = userId;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}