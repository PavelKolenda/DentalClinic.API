namespace DentalClinic.Shared.Pagination;

public static class PagedListExtensions<T>
{
    public static PagedList<T> Create(IQueryable<T> source, int page, int pageSize)
    {
        int totalCount = source.Count();

        var items = source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedList<T>(items, page, pageSize, totalCount);
    }
}