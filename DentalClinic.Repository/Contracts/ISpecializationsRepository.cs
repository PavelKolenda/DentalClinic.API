using DentalClinic.Models.Entities;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.Pagination;

namespace DentalClinic.Repository.Contracts;
public interface ISpecializationsRepository
{
    Task<Specialization> CreateAsync(Specialization specialization);
    Task DeleteAsync(int id);
    IQueryable<Specialization> GetAll();
    Task<Specialization> GetByIdAsync(int id, bool trackChanges);
    Task<Specialization> GetByNameAsync(string name);
    PagedList<Specialization> GetPaged(QueryParameters query);
    Task UpdateAsync(int id, Specialization specialization);
}
