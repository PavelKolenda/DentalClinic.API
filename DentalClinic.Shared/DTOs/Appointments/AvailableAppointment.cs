namespace DentalClinic.Shared.DTOs.Appointments;

public class AvailableAppointment
{
    public int AppointmentId { get; set; }
    public TimeOnly AvailableTime { get; set; }
}