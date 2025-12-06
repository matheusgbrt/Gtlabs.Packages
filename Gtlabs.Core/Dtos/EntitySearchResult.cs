namespace Gtlabs.Core.Dtos;

public class EntitySearchResult<T>
{
    public T Entity { get; set; } = default!;
    public bool Found { get; set; }
}