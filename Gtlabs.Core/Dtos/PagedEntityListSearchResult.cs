namespace Gtlabs.Core.Dtos;

public class PagedEntityListSearchResult<T> : EntityListSearchResult<T>
{
    public int TotalCount { get; set; }
}