using DentalClinic.Models.Entities;
using DentalClinic.Repository.Contracts;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.Notifications;

using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Services;
public class AppNotificationsSender : INotificationsSenderService
{
    private readonly INotificationsRepository _notificationsRepository;
    private readonly IPatientsRepository _patientsRepository;

    public AppNotificationsSender(INotificationsRepository notificationsRepository, IPatientsRepository patientsRepository)
    {
        _notificationsRepository = notificationsRepository;
        _patientsRepository = patientsRepository;
    }

    public async Task SendToAllPatientsAsync(NotificationCreateDto notificationDto)
    {
        var patientsId = await _patientsRepository.GetAll().Select(x => x.Id).ToListAsync();
        DateTime now = DateTime.UtcNow.AddHours(3);

        Notification notification = new()
        {
            Article = notificationDto.Article,
            Text = notificationDto.Text,
            SandedAt = now
        };

        foreach (int patientId in patientsId)
        {
            notification.PatientId = patientId;
            await _notificationsRepository.CreateAsync(notification);
        }
    }

    public async Task SendToPatientAsync(int patientId, NotificationCreateDto notificationDto)
    {
        Notification notification = new()
        {
            Article = notificationDto.Article,
            Text = notificationDto.Text,
            SandedAt = DateTime.Now,
            PatientId = patientId
        };

        await _notificationsRepository.CreateAsync(notification);
    }
}

