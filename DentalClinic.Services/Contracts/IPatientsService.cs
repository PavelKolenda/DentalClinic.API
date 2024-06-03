using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.DTOs.Appointments;
using DentalClinic.Shared.DTOs.Patients;
using DentalClinic.Shared.DTOs.Roles;
using DentalClinic.Shared.Pagination;

namespace DentalClinic.Services.Contracts;
public interface IPatientsService
{
    Task DeleteAsync(int id);
    PagedList<AppointmentDto> GetAppointments(QueryParameters query);
    Task<PatientDto> GetByIdAsync(int id);
    PagedList<PatientDto> GetPaged(QueryParameters query);
    Task UpdateAsync(PatientUpdateDto patientUpdateDto, int id);
    Task UpdateRoles(int id, RoleDto roleDto);
}
