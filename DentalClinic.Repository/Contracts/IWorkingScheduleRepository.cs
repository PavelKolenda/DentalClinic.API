using DentalClinic.Models.Entities;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.Pagination;

namespace DentalClinic.Repository.Contracts
{
    public interface IWorkingScheduleRepository
    {
        Task<WorkingSchedule> CreateAsync(WorkingSchedule workingSchedule);
        Task DeleteAsync(int id);
        IQueryable<WorkingSchedule> GetAll();
        Task<WorkingSchedule> GetById(int id);
        PagedList<WorkingSchedule> GetPaged(QueryParameters query);
        Task UpdateAsync(int id, WorkingSchedule workingSchedule);
    }
}