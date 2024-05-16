using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.DTOs.Dentists;
using DentalClinic.Shared.Pagination;

namespace DentalClinic.Services.Contracts;
public interface IDentistsService
{
    Task AddWorkingSchedule(int dentistId, int workingScheduleId);
    Task<DentistDto> CreateAsync(DentistCreateDto dentistDto);
    Task DeleteAsync(int id);
    Task DeleteWorkingSchedule(int dentistId, int workingScheduleId);
    PagedList<DentistDto> GetPaged(QueryParameters query);
    Task UpdateAsync(DentistUpdateDto dentistDto, int id);
}
