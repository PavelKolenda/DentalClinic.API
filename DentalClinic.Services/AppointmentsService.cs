using DentalClinic.Models.Entities;
using DentalClinic.Models.Exceptions;
using DentalClinic.Repository.Contracts;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs;
using DentalClinic.Shared.DTOs.Appointments;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DentalClinic.Services;
public class AppointmentsService : IAppointmentsService
{
    private readonly IAppointmentsRepository _appointmentsRepository;
    private readonly ILogger<AppointmentsService> _logger;
    private readonly IPatientsRepository _patientsRepository;
    private readonly IDentistRepository _dentistRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppointmentsService(IAppointmentsRepository appointmentsRepository,
                               ILogger<AppointmentsService> logger,
                               IPatientsRepository patientsRepository,
                               IDentistRepository dentistRepository,
                               IHttpContextAccessor httpContextAccessor)
    {
        _appointmentsRepository = appointmentsRepository;
        _logger = logger;
        _patientsRepository = patientsRepository;
        _dentistRepository = dentistRepository;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<IEnumerable<AvailableAppointmentsDto>> GetAvailableForMonthAsync(int dentistId)
    {
        var dentist = await _dentistRepository.GetByIdAsync(dentistId, false);

        var appointments = await _appointmentsRepository.GetAvailableAsync(dentist.Id);

        var availableAppointments = appointments
            .GroupBy(a => new { a.DentistId, Date = DateOnly.FromDateTime(a.Date) })
            .Select(g => new AvailableAppointmentsDto
            {
                DentistId = g.Key.DentistId,
                Date = g.Key.Date,
                AvailableAppointments = g.Select(a => new AvailableAppointment
                {
                    AppointmentId = a.Id,
                    AvailableTime = TimeOnly.FromDateTime(a.Date).AddHours(3),
                }),
                Count = g.Count()
            });

        return availableAppointments;
    }

    public async Task<AppointmentDto> MakeAppointmentAsync(int dentistId, int appointmentId)
    {
        int patientId = GetPatientIdFromClaims();

        Dentist dentist = await _dentistRepository.GetByIdAsync(dentistId, false);
        Appointment appointment = await _appointmentsRepository.GetById(appointmentId);
        Patient patient = await _patientsRepository.GetById(patientId, false);

        await _appointmentsRepository.AssignPatientAsync(patient, appointment);

        var createdAppointment = await GetById(appointmentId);

        return createdAppointment;
    }

    public async Task CancelAppointmentAsync(int appointmentId)
    {
        int patientId = GetPatientIdFromClaims();

        Appointment appointment = await _appointmentsRepository.GetById(appointmentId);

        if (appointment.PatientId != patientId)
        {
            throw new UnauthorizedAccessException();
        }

        if (appointment.Date < DateTime.UtcNow)
        {
            throw new InvalidRequestException("Can't delete passed appointment");
        }

        await _appointmentsRepository.DeleteAsync(appointmentId, patientId);
    }

    public async Task<AppointmentDto> GetById(int appointmentId)
    {
        var patientId = GetPatientIdFromClaims();

        var appointment = await _appointmentsRepository
            .GetAll()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(a => a.Patient)
            .Include(d => d.Dentist)
                .ThenInclude(d => d.Specialization)
            .FirstAsync(x => x.Id == appointmentId && x.PatientId == patientId);

        AppointmentDto createdAppointment = new()
        {
            AppointmentId = appointmentId,
            DentistName = appointment.Dentist.Name,
            DentistSurname = appointment.Dentist.Surname,
            DentistPatronymic = appointment.Dentist.Patronymic,
            DentistCabinetNumber = appointment.Dentist.CabinetNumber,
            DentistSpecialization = appointment.Dentist.Specialization.Name,
            PatientName = appointment.Patient.Name,
            PatientSurname = appointment.Patient.Surname,
            PatientPatronymic = appointment.Patient.Patronymic,
            AppointmentDate = DateOnly.FromDateTime(appointment.Date),
            AppointmentTime = TimeOnly.FromDateTime(appointment.Date.AddHours(3))
        };

        return createdAppointment;
    }

    private int GetPatientIdFromClaims()
    {
        var patientIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("Id");

        if (patientIdClaim == null)
        {
            throw new UnauthorizedAccessException("Patient isn't authorized");
        }

        int patientId = Convert.ToInt32(patientIdClaim.Value);
        return patientId;
    }
}
