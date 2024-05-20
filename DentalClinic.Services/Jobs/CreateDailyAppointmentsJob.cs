using DentalClinic.Models.Entities;
using DentalClinic.Repository.Contracts;
using DentalClinic.Services.Options;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Quartz;

namespace DentalClinic.Services.Jobs;

[DisallowConcurrentExecution]
public class CreateDailyAppointmentsJob : CreateAppointmentsJobBase, IJob
{
    private readonly IAppointmentsRepository _appointmentsRepository;
    private readonly IDentistRepository _dentistRepository;
    private readonly ILogger<CreateDailyAppointmentsJob> _logger;
    private readonly CreateAppointmentsOptions _options;

    public CreateDailyAppointmentsJob(IAppointmentsRepository appointmentsRepository,
                                            ILogger<CreateDailyAppointmentsJob> logger,
                                            IOptions<CreateAppointmentsOptions> options,
                                            IDentistRepository dentistRepository)
    {
        _appointmentsRepository = appointmentsRepository;
        _logger = logger;
        _options = options.Value;
        _dentistRepository = dentistRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var dayOfWeek = GetDayOfWeekAsString(DateTime.UtcNow.DayOfWeek);

        var dentists = await _dentistRepository
            .GetAll()
            .Include(ws => ws.WorkingSchedule)
            .Where(d => d.WorkingSchedule.Any(ws => ws.WorkingDay == dayOfWeek))
             .Select(d => new
             {
                 d.Id,
                 WorkingSchedule = d.WorkingSchedule.FirstOrDefault(ws => ws.WorkingDay == dayOfWeek)
             })
            .ToListAsync();

        List<Appointment> appointments = [];

        foreach (var dentist in dentists)
        {
            var workingSchedule = dentist.WorkingSchedule;

            if (workingSchedule == null)
            {
                _logger.LogWarning("");
                return;
            }

            int workingDayInMinutes = Convert.ToInt32((workingSchedule.End - workingSchedule.Start).TotalMinutes);

            if (workingDayInMinutes < 0)
            {
                _logger.LogWarning("");
                return;
            }

            int appointmentsCount = workingDayInMinutes / _options.TimeToOneAppointment.Minute;

            appointments.AddRange(CreateAppointments(dentist.Id,
                                                     workingSchedule.Start,
                                                     DateTime.UtcNow.AddDays(31),
                                                     appointmentsCount,
                                                     _options.TimeToOneAppointment.Minute));
        }

        await _appointmentsRepository.CreateAppointments(appointments);
    }
}