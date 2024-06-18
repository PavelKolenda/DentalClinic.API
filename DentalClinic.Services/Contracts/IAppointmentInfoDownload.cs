namespace DentalClinic.Services.Contracts;

public interface IAppointmentInfoDownload
{
    Task<byte[]> Download(int appointmentId);
}