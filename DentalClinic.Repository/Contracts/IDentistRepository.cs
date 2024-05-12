using DentalClinic.Models.Entities;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.Pagination;

namespace DentalClinic.Repository.Contracts;
public interface IDentistRepository
{
    Task<Dentist> CreateAsync(Dentist dentist, int specializationId);
    Task DeleteAsync(int id);
    IQueryable<Dentist> GetAll();
    PagedList<Dentist> GetPaged(QueryParameters query);
    Task UpdateAsync(int id, Dentist dentist);
}
