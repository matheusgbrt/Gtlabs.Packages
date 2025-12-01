namespace Gtlabs.Persistence.Dtos;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = default!;
    public int TotalCount { get; set; }
}