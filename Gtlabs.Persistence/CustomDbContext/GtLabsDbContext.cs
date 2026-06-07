using System.Reflection;
using Gtlabs.Core.Extensions;
using Gtlabs.Persistence.DataFilters;
using Gtlabs.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gtlabs.Persistence.CustomDbContext;

public abstract class GtLabsDbContext : DbContext
{
    private readonly IDataFilter<ISoftDelete> _softDeleteFilter;

    protected GtLabsDbContext(
        DbContextOptions options,
        IDataFilter<ISoftDelete>? softDeleteFilter = null) : base(options)
    {
        _softDeleteFilter = softDeleteFilter ?? new DataFilter<ISoftDelete>();
    }

    protected bool IsSoftDeleteFilterEnabled => _softDeleteFilter.IsEnabled;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // TABLE
            entity.SetTableName(entity!.GetTableName()!.ToSnakeCase());

            // COLUMNS
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
            }

            // KEYS (PK)
            foreach (var key in entity.GetKeys())
            {
                key.SetName(key.GetName()!.ToSnakeCase());
            }

            // FOREIGN KEYS
            foreach (var fk in entity.GetForeignKeys())
            {
                fk.SetConstraintName(fk.GetConstraintName()!.ToSnakeCase());
            }
        }

        ApplySoftDeleteDataFilter(modelBuilder);
    }

    private void ApplySoftDeleteDataFilter(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(SoftDeleteEntity).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(GtLabsDbContext)
                    .GetMethod(
                        nameof(ApplySoftDeleteDataFilterForEntity),
                        BindingFlags.Instance | BindingFlags.NonPublic,
                        binder: null,
                        types: [typeof(ModelBuilder)],
                        modifiers: null)!
                    .MakeGenericMethod(entityType.ClrType);

                method.Invoke(this, [modelBuilder]);
            }
        }
    }

    private void ApplySoftDeleteDataFilterForEntity<TEntity>(ModelBuilder modelBuilder)
        where TEntity : SoftDeleteEntity
    {
        modelBuilder.Entity<TEntity>()
            .HasQueryFilter(entity => !IsSoftDeleteFilterEnabled || !entity.IsDeleted);
    }
}
