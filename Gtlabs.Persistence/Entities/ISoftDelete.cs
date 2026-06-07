namespace Gtlabs.Persistence.Entities;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
    Guid? DeleterId { get; set; }
    DateTime? DeletionTime { get; set; }
}
