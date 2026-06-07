using Gtlabs.AmbientData.Interfaces;
using Gtlabs.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Gtlabs.Persistence.Interceptors;

public class AuditedEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IAmbientData _ambientData;

    public AuditedEntitySaveChangesInterceptor(IAmbientData ambientData)
    {
        _ambientData = ambientData;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyEntityChanges(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyEntityChanges(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyEntityChanges(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var changeContext = new EntityChangeContext(_ambientData.GetUserId(), DateTime.UtcNow);

        foreach (var entry in context.ChangeTracker.Entries<AuditedEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    ApplyAddedValues(entry.Entity, changeContext);
                    break;

                case EntityState.Modified:
                    ApplyModifiedValues(entry.Entity, changeContext);
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    ApplyDeletedValues(entry.Entity, changeContext);
                    break;
            }
        }
    }

    private static void ApplyAddedValues(AuditedEntity entity, EntityChangeContext context)
    {
        entity.CreationTime = context.Now;
        entity.CreatorId = context.UserId;
        entity.LastModificationTime = context.Now;
        entity.ModifierId = context.UserId;
        entity.IsDeleted = false;
    }

    private static void ApplyModifiedValues(AuditedEntity entity, EntityChangeContext context)
    {
        entity.LastModificationTime = context.Now;
        entity.ModifierId = context.UserId;
    }

    private static void ApplyDeletedValues(AuditedEntity entity, EntityChangeContext context)
    {
        entity.IsDeleted = true;
        entity.DeleterId = context.UserId;
        entity.DeletionTime = context.Now;
        ApplyModifiedValues(entity, context);
    }

    private readonly record struct EntityChangeContext(Guid? UserId, DateTime Now);
}
