using DentalClinic.Shared.DTOs;
using DentalClinic.Shared.DTOs.Appointments;

namespace DentalClinic.Services.Contracts;
public interface IAppointmentsService
{
    Task CancelAppointmentAsync(int appointmentId);
    Task<IEnumerable<AvailableAppointmentsDto>> GetAvailableForMonthAsync(int dentistId);
    Task<AppointmentDto> GetById(int appointmentId);
    Task<AppointmentDto> MakeAppointmentAsync(int dentistId, int appointmentId);
    Task<AppointmentDto> PatientReenrollment(int patientId, int appointmentId);
}
