namespace Gtlabs.Persistence.Entities;

public abstract class AuditedEntity : SoftDeleteEntity
{
    public Guid? CreatorId { get; set; }
    public Guid? ModifierId { get; set; }
    public DateTime? CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }
}