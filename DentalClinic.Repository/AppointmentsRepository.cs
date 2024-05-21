using DentalClinic.Models.Entities;
using DentalClinic.Models.Exceptions;
using DentalClinic.Repository.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DentalClinic.Repository;
public class AppointmentsRepository : IAppointmentsRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<AppointmentsRepository> _logger;
    public AppointmentsRepository(ClinicDbContext context,
                                  ILogger<AppointmentsRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IQueryable<Appointment> GetAll()
    {
        var appointments = _context.Appointments.AsQueryable();

        return appointments;
    }

    public async Task<IEnumerable<Appointment>> GetAvailableAsync(int dentistId)
    {
        DateTime today = DateTime.UtcNow;

        var appointments = await _context.Appointments
            .AsNoTracking()
            .Include(a => a.Dentist)
            .Where(a => a.Date >= today && a.Date <= today.AddDays(31) && a.PatientId == null && a.DentistId == dentistId)
            .ToListAsync();

        return appointments;
    }

    public async Task<Appointment> CreateAsync(Appointment appointment)
    {
        await _context.Appointments.AddAsync(appointment);

        await _context.SaveChangesAsync();

        return appointment;
    }

    public async Task CreateAsync(IEnumerable<Appointment> appointments)
    {
        foreach (var appointment in appointments)
        {
            await _context.Appointments.AddAsync(appointment);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<Appointment> GetById(int id)
    {
        Appointment? appointment = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == id);

        if (appointment is null)
        {
            throw new NotFoundException($"Appointment with Id:{id} don't exists");
        }

        return appointment;
    }

    public async Task DeleteAsync(int appointmentId, int patientId)
    {
        await _context.Appointments.Where(a => a.Id == appointmentId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.PatientId, (int?)null)
            );

        await _context.SaveChangesAsync();
    }

    public async Task AssignPatientAsync(Patient patient, Appointment appointment)
    {
        appointment.PatientId = patient.Id;
        await _context.SaveChangesAsync();
    }
}
