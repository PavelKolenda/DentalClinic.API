namespace DentalClinic.Services.Options;
public class CreateAppointmentsOptions
{
    public TimeOnly TimeToOneAppointment { get; set; } = new TimeOnly(0, 30, 0);
}