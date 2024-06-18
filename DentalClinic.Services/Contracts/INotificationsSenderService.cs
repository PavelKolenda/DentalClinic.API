using DentalClinic.Shared.DTOs.Notifications;

namespace DentalClinic.Services.Contracts;
public interface INotificationsSenderService
{
    Task SendToPatientAsync(int patientId, NotificationCreateDto notificationDto);
    Task SendToAllPatientsAsync(NotificationCreateDto notificationDto);
}