using Gtlabs.Core.Enums;

namespace Gtlabs.Core.Dtos;

public class EntityAlterationResult<T>
{
    public bool Success { get; set; }
    public T Entity { get; set; } = default!;
    public EntityAlterationError Error { get; set; }
    public string ErrorMessage { get; set; } = String.Empty;
}