using DentalClinic.Shared.DTOs.Appointments;

namespace DentalClinic.Shared.DTOs;
public class AvailableAppointmentsDto
{
    public int DentistId { get; set; }
    public DateOnly Date { get; set; }
    public IEnumerable<AvailableAppointment> AvailableAppointments { get; set; }
    public int Count { get; set; }
}