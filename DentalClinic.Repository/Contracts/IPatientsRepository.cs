using DentalClinic.Models.Entities;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.Pagination;

namespace DentalClinic.Repository.Contracts;
public interface IPatientsRepository
{
    Task<Patient> CreateAsync(Patient patient);
    Task DeleteAsync(int id);
    IQueryable<Patient> GetAll();
    PagedList<Patient> GetPaged(QueryParameters query);
    Task UpdateAsync(int id, Patient patient);
}
