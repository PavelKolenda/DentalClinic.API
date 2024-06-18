using System.Text;

using DentalClinic.Repository.Contracts;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.Notifications;

using Microsoft.EntityFrameworkCore;

using Quartz;

namespace DentalClinic.Services.Jobs.Notifications;

public class SendNotificationsOneDayBeforeAppointmentJob : IJob
{
    private readonly IAppointmentsRepository _appointmentsRepository;
    private readonly INotificationsSenderService _notificationsSenderService;

    public SendNotificationsOneDayBeforeAppointmentJob(IAppointmentsRepository appointmentsRepository,
                                                    INotificationsSenderService notificationsSenderService)
    {
        _appointmentsRepository = appointmentsRepository;
        _notificationsSenderService = notificationsSenderService;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        int timeDifferenceBetweenUtcAndLocal = 3;

        var dateOnly = DateOnly.FromDateTime(DateTime.UtcNow);

        DateTime now = dateOnly.ToDateTime(new TimeOnly(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0))
            .AddHours(timeDifferenceBetweenUtcAndLocal);

        var appointments = await _appointmentsRepository.GetAll()
            .Include(p => p.Patient)
            .Include(d => d.Dentist)
            .Where(x => x.Date.Date == now.Date.Date.AddDays(1) && x.PatientId != null)
            .ToListAsync();

        NotificationCreateDto notification = new()
        {
            Article = "Напоминание о записи",
        };

        StringBuilder sb = new();

        foreach (var appointment in appointments)
        {
            var patient = appointment.Patient!;
            var dentist = appointment.Dentist!;

            sb.Clear();
            sb.AppendLine($"Уважаемый {patient.Surname} {patient.Name} {patient?.Patronymic}!");
            sb.AppendLine($"Напоминаем вам о записи к врачу {dentist.Surname} {dentist.Name} {dentist?.Patronymic}");
            sb.AppendLine($"На {DateOnly.FromDateTime(appointment.Date)} в {TimeOnly.FromDateTime(appointment.Date)}");
            notification.Text = sb.ToString();

            await _notificationsSenderService.SendToPatientAsync(patient.Id, notification);
        }
    }
}