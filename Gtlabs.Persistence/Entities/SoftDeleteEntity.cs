namespace Gtlabs.Persistence.Entities;

public abstract class SoftDeleteEntity : Entity, ISoftDelete
{
    public bool IsDeleted { get; set; }
    public Guid? DeleterId { get; set; }
    public DateTime? DeletionTime { get; set; }
}
