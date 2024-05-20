using DentalClinic.Models.Entities;
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

    public async Task<Appointment> CreateAppointment(Appointment appointment)
    {
        await _context.Appointments.AddAsync(appointment);

        await _context.SaveChangesAsync();

        return appointment;
    }

    public async Task CreateAppointments(IEnumerable<Appointment> appointments)
    {
        foreach (var appointment in appointments)
        {
            await _context.Appointments.AddAsync(appointment);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAppointment(int appointmentId, int patientId)
    {
        await _context.Appointments.Where(a => a.Id == appointmentId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.PatientId, (int?)null)
            );

        await _context.SaveChangesAsync();
    }
}
