using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.DTOs.WorkingSchedules;
using DentalClinic.Shared.Pagination;

namespace DentalClinic.Services.Contracts
{
    public interface IWorkingScheduleService
    {
        Task<WorkingScheduleDto> CreateAsync(WorkingScheduleCreateDto workingScheduleCreateDto);
        Task DeleteAsync(int id);
        Task<WorkingScheduleDto> GetByIdAsync(int id);
        PagedList<WorkingScheduleDto> GetPaged(QueryParameters query, string? dayFilter);
        Task UpdateAsync(int id, WorkingScheduleUpdateDto wsUpdateDto);
    }
}