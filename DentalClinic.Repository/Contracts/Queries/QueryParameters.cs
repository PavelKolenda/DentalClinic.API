using DentalClinic.Shared.Pagination;
using DentalClinic.Shared.Sorting;

namespace DentalClinic.Repository.Contracts.Queries;
public class QueryParameters : IPagedQuery, ISortedQuery
{
    private int _pageSize;
    public int Page { get; set; }
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = _pageSize > 1000 ? 100 : value;
        }
    }
    public string? SortColumn { get; set; }
    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
}