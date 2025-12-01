namespace Gtlabs.Core.Dtos;

public class EntitySearchResult<T>
{
    public T Entity { get; set; }
    public bool Found { get; set; }
}