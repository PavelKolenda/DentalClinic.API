using DentalClinic.Models.Entities;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.Pagination;

namespace DentalClinic.Repository.Contracts;
public interface IDentistRepository
{
    Task AddWorkingSchedule(Dentist dentist, WorkingSchedule workingSchedule);
    Task<Dentist> CreateAsync(Dentist dentist, int specializationId);
    Task DeleteAsync(int id);
    Task DeleteWorkingScheduleAsync(Dentist dentist, WorkingSchedule workingSchedule);
    IQueryable<Dentist> GetAll();
    PagedList<Dentist> GetPaged(QueryParameters query);
    Task UpdateAsync(int id, Dentist dentist);
}
