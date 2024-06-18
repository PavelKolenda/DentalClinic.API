using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.DTOs.News;
using DentalClinic.Shared.Pagination;

namespace DentalClinic.Services.Contracts
{
    public interface INewsService
    {
        Task<NewsDto> CreateAsync(NewsCreateDto newsDto);
        Task DeleteAsync(int id);
        Task<NewsDto> GetByIdAsync(int id);
        PagedList<NewsDto> GetPagedList(QueryParameters query);
        Task UpdateAsync(int id, NewsUpdateDto newsDto);
    }
}