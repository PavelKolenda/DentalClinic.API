using DentalClinic.Models.Entities;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.Pagination;

namespace DentalClinic.Repository.Contracts
{
    public interface INewsRepository
    {
        Task<News> CreateAsync(News news);
        Task DeleteAsync(int id);
        Task<News> GetById(int id);
        PagedList<News> GetPaged(QueryParameters query);
        Task UpdateAsync(int id, News news);
    }
}