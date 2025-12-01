namespace Gtlabs.Persistence.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Page<T>(this IQueryable<T> query, int page, int pageSize)
        => query.Skip((page - 1) * pageSize).Take(pageSize);
}