using System.Linq.Expressions;

using DentalClinic.Models.Entities;
using DentalClinic.Models.Exceptions;
using DentalClinic.Repository.Contracts;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.Pagination;
using DentalClinic.Shared.Sorting;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DentalClinic.Repository;
public class DentistRepository : IDentistRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<DentistRepository> _logger;
    private readonly IAppointmentsRepository _appointmentsRepository;

    public DentistRepository(ClinicDbContext context,
                             ILogger<DentistRepository> logger,
                             IAppointmentsRepository appointmentsRepository)
    {
        _context = context;
        _logger = logger;
        _appointmentsRepository = appointmentsRepository;
    }

    public IQueryable<Dentist> GetAll()
    {
        return _context.Dentists.AsQueryable();
    }

    public async Task<Dentist> CreateAsync(Dentist dentist, int specializationId)
    {
        dentist.SpecializationId = specializationId;

        await _context.Dentists.AddAsync(dentist);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Create Dentist with Id:{Id}", dentist.Id);
        return dentist;
    }

    public async Task<Dentist> GetByIdAsync(int id, bool trackChanges)
    {
        IQueryable<Dentist> query = _context.Dentists;
        if (!trackChanges)
        {
            query = query.AsNoTracking();
        }

        Dentist? dentist = await query.FirstOrDefaultAsync(x => x.Id == id);

        if (dentist is null)
        {
            throw new NotFoundException($"Dentist with id:{id} doesn't exist");
        }

        return dentist;
    }

    public async Task DeleteAsync(int id)
    {
        await _context.Dentists.Where(p => p.Id == id).ExecuteDeleteAsync();
        _logger.LogInformation("Delete Dentist with Id:{id}", id);
    }

    public async Task AddWorkingSchedule(Dentist dentist, WorkingSchedule workingSchedule)
    {
        dentist.WorkingSchedule.Add(workingSchedule);

        List<Appointment> appointments = [];

        int workingDayInMinutes = Convert.ToInt32((workingSchedule.End - workingSchedule.Start).TotalMinutes);
        int appointmentsCount = workingDayInMinutes / TimeToOneAppointment.Minute;

        for (int i = 1; i < 32; i++)
        {
            var date = DateTime.UtcNow.AddDays(i);

            var dayOfWeek = GetDayOfWeekAsString(date.DayOfWeek);

            if (dayOfWeek != workingSchedule.WorkingDay)
                continue;

            MutableTimeOnly appointmentTime = new(workingSchedule.Start);
            MutableDateTime appointmentDate = new(DateTime.UtcNow.AddDays(i));

            for (int j = 0; j < appointmentsCount; j++)
            {
                Appointment appointment = new()
                {
                    Date = new DateTime(DateOnly.FromDateTime(appointmentDate.Date), appointmentTime.Time),
                    DentistId = dentist.Id,
                };

                appointments.Add(appointment);

                appointmentTime.AddMinutes(TimeToOneAppointment.Minute);
            }

        }
        await _appointmentsRepository.CreateAsync(appointments);

        await _context.SaveChangesAsync();
    }

    private TimeOnly TimeToOneAppointment { get; set; } = new TimeOnly(0, 30, 0);

    protected string GetDayOfWeekAsString(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => "понедельник",
            DayOfWeek.Tuesday => "вторник",
            DayOfWeek.Wednesday => "среда",
            DayOfWeek.Thursday => "четверг",
            DayOfWeek.Friday => "пятница",
            DayOfWeek.Saturday => "суббота",
            DayOfWeek.Sunday => "воскресенье"
        };
    }

    public async Task DeleteWorkingScheduleAsync(Dentist dentist, WorkingSchedule workingSchedule)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        var appointments = await _context.Appointments
               .Include(a => a.Dentist)
               .Where(a => a.DentistId == dentist.Id && DateOnly.FromDateTime(a.Date) >= now
               && DateOnly.FromDateTime(a.Date) <= now.AddDays(30))
               .ToListAsync();

        List<Appointment> appointmentsToDelete = [];

        foreach (var appointment in appointments)
        {
            if (GetDayOfWeekAsString(appointment.Date.Date.DayOfWeek) == workingSchedule.WorkingDay)
            {
                appointmentsToDelete.Add(appointment);
            }
        }

        dentist.WorkingSchedule.Remove(workingSchedule);

        _context.Appointments.RemoveRange(appointmentsToDelete);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, Dentist dentist)
    {
        await _context.Dentists.Where(d => d.Id == id)
            .ExecuteUpdateAsync(s => s
            .SetProperty(d => d.Name, dentist.Name)
            .SetProperty(d => d.Surname, dentist.Surname)
            .SetProperty(d => d.Patronymic, dentist.Patronymic)
            .SetProperty(d => d.CabinetNumber, dentist.CabinetNumber)
            .SetProperty(d => d.SpecializationId, dentist.SpecializationId)
            );

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated Dentist with Id:{id}", id);
    }

    public PagedList<Dentist> GetPaged(QueryParameters query)
    {
        var dbQuery = _context.Dentists
            .Include(s => s.Specialization)
            .AsQueryable();

        if (query.SortOrder is not null)
        {
            if (query.SortOrder == SortOrder.Ascending)
            {
                dbQuery = dbQuery.OrderBy(GetSortColumn(query));
            }
            else
            {
                dbQuery = dbQuery.OrderByDescending(GetSortColumn(query));
            }
        }

        return PagedListExtensions<Dentist>.Create(dbQuery, query.Page, query.PageSize);
    }

    public PagedList<Appointment> GetAppointmentsList(int dentistId, QueryParameters query, DateOnly specificDate)
    {
        var appointments = _context.Appointments
            .AsNoTracking()
            .AsQueryable()
            .AsSingleQuery()
            .Include(x => x.Patient)
            .Include(d => d.Dentist)
                .ThenInclude(s => s.Specialization)
            .Where(x => x.DentistId == dentistId
            && x.PatientId != null
            && DateOnly.FromDateTime(x.Date) == specificDate)
            .OrderBy(x => x.Date);

        return PagedListExtensions<Appointment>.Create(appointments, query.Page, query.PageSize);
    }

    private static Expression<Func<Dentist, object>> GetSortColumn(QueryParameters query)
    {
        return query.SortColumn?.ToLowerInvariant()
            switch
        {
            "name" => p => p.Name,
            "surname" => p => p.Surname,
            "patronymic" => p => p.Patronymic,
            _ => p => p.Name
        };
    }
}
class MutableTimeOnly
{
    public TimeOnly Time { get; private set; }

    public MutableTimeOnly(TimeOnly time)
    {
        Time = time;
    }

    public void AddMinutes(int minutes)
    {
        Time = Time.AddMinutes(minutes);
    }

    public void AddHours(int hours)
    {
        Time = Time.AddHours(hours);
    }
}

class MutableDateTime
{
    public DateTime Date { get; private set; }

    public MutableDateTime(DateTime date)
    {
        Date = date;
    }

    public void AddDays(int days)
    {
        Date = Date.AddDays(days);
    }
}