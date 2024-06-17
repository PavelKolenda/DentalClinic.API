using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.DTOs.Appointments;
using DentalClinic.Shared.DTOs.Dentists;
using DentalClinic.Shared.DTOs.WorkingSchedules;
using DentalClinic.Shared.Pagination;

namespace DentalClinic.Services.Contracts;
public interface IDentistsService
{
    Task AddWorkingSchedule(int dentistId, int workingScheduleId);
    Task<DentistDto> CreateAsync(DentistCreateDto dentistDto);
    Task DeleteAsync(int id);
    Task DeleteWorkingSchedule(int dentistId, int workingScheduleId);
    PagedList<AppointmentDto> GetAppointmentsList(QueryParameters query, DateOnly specificDate);
    Task<IEnumerable<DentistDto>> GetBySpecialization(int specializationId);
    Task<DentistDtoAsUser> GetDentistAsync(int id);
    PagedList<DentistDto> GetPaged(QueryParameters query);
    Task<WorkingScheduleDtoToReturn> GetWorkingScheduleAsync(int dentistId);
    Task UpdateAsync(DentistUpdateDto dentistDto, int id);
}
