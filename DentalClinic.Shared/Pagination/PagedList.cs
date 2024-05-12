using System.Text.Json.Serialization;

namespace DentalClinic.Shared.Pagination;
public class PagedList<T>
{
    [JsonPropertyName("items")]
    public List<T> Items { get; set; }
    [JsonPropertyName("page")]
    public int Page { get; set; }
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }
    [JsonPropertyName("hasPreviousPage")]
    public bool HasPreviousPage => PageSize > 1;
    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage => Page * PageSize < TotalCount;

    public PagedList(List<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}