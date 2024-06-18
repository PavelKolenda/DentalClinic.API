using DentalClinic.Models.Entities;
using DentalClinic.Models.Exceptions;
using DentalClinic.Repository.Contracts;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Services.Auth;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.Appointments;
using DentalClinic.Shared.DTOs.Dentists;
using DentalClinic.Shared.DTOs.WorkingSchedules;
using DentalClinic.Shared.Pagination;

using Mapster;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Services;
public class DentistsService : IDentistsService
{
    private readonly IDentistRepository _dentistRepository;
    private readonly ISpecializationsRepository _specializationsRepository;
    private readonly IWorkingScheduleRepository _workingScheduleRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRoleRepository _roleRepository;
    private readonly IPatientsRepository _patientsRepository;
    private readonly IPasswordHasher _passwordHasher;

    public DentistsService(IDentistRepository dentistRepository,
                           ISpecializationsRepository specializationsRepository,
                           IWorkingScheduleRepository workingScheduleRepository,
                           IHttpContextAccessor httpContextAccessor,
                           IRoleRepository roleRepository,
                           IPatientsRepository patientsRepository,
                           IPasswordHasher passwordHasher)
    {
        _dentistRepository = dentistRepository;
        _specializationsRepository = specializationsRepository;
        _workingScheduleRepository = workingScheduleRepository;
        _httpContextAccessor = httpContextAccessor;
        _roleRepository = roleRepository;
        _patientsRepository = patientsRepository;
        _passwordHasher = passwordHasher;
    }

    public PagedList<DentistDto> GetPaged(QueryParameters query)
    {
        var dentists = _dentistRepository.GetPaged(query);

        var dentistsDto = dentists.Items.Adapt<List<DentistDto>>();

        return new PagedList<DentistDto>(dentistsDto, dentists.Page, dentists.PageSize, dentists.TotalCount);
    }

    public async Task<IEnumerable<DentistDto>> GetBySpecialization(int specializationId)
    {
        Specialization specialization = await _specializationsRepository.GetByIdAsync(specializationId, false);

        var dentists = await _dentistRepository
            .GetAll()
            .AsNoTracking()
            .Include(s => s.Specialization)
            .Where(x => x.SpecializationId == specialization.Id)
            .ToListAsync();

        List<DentistDto> dentistsDto = dentists.Adapt<List<DentistDto>>();

        return dentistsDto;
    }

    public async Task<DentistDto> CreateAsync(DentistCreateDto dentistDto)
    {
        if (await IsEmailExists(dentistDto.Email))
        {
            throw new InvalidRequestException($"Provided Email: {dentistDto.Email} already exists");
        }

        var specialization = await _specializationsRepository.GetByNameAsync(dentistDto.Specialization);
        var role = await _roleRepository.GetByName("Dentist");

        Dentist dentist = dentistDto.Adapt<Dentist>();

        await _dentistRepository.CreateAsync(dentist, specialization.Id);

        Patient patient = new()
        {
            Name = dentist.Name,
            Surname = dentist.Surname,
            Patronymic = dentist.Patronymic,
            Email = dentistDto.Email,
            PasswordHash = _passwordHasher.Generate(dentistDto.Password),
            BirthDate = dentistDto.BirthDate,
            PhoneNumber = dentistDto.PhoneNumber,
            Address = dentistDto.Address
        };

        await _patientsRepository.CreateAsync(patient, role);

        DentistDto dentistToReturn = dentist.Adapt<DentistDto>();

        return dentistToReturn;
    }

    private async Task<bool> IsEmailExists(string email)
    {
        return await _patientsRepository
            .GetAll()
            .Select(e => e.Email)
            .AsNoTracking()
            .AnyAsync(p => p == email);
    }

    public async Task DeleteAsync(int id)
    {
        var dentist = await _dentistRepository.GetAll()
            .Include(s => s.Specialization)
            .FirstOrDefaultAsync(x => x.Id == id);

        var patient = await GetDentistFromPatients(dentist);

        await _patientsRepository.DeleteAsync(patient.Id);
        await _dentistRepository.DeleteAsync(id);
    }


    private async Task<Patient> GetDentistFromPatients(Dentist dentist)
    {
        var patient = await _patientsRepository.GetAll()
            .Include(r => r.Roles)
            .FirstOrDefaultAsync(x => x.Name == dentist.Name
            && x.Surname == dentist.Surname
            && x.Patronymic == dentist.Patronymic
            && x.Roles.Any(r => r.Name == "Dentist"));

        if (patient == null)
        {
            throw new InvalidRequestException("Dentist exists, but patient don't");
        }

        return patient;
    }

    public async Task<DentistDtoAsUser> GetDentistAsync(int id)
    {
        var dentist = await _dentistRepository.GetAll()
            .Include(s => s.Specialization)
            .FirstOrDefaultAsync(x => x.Id == id);

        var patient = await GetDentistFromPatients(dentist);

        DentistDtoAsUser dentistDtoAsUser = new()
        {
            Name = dentist.Name,
            Surname = dentist.Surname,
            Patronymic = dentist.Patronymic,
            CabinetNumber = dentist.CabinetNumber,
            Specialization = dentist.Specialization.Name,
            BirthDate = patient.BirthDate,
            Email = patient.Email,
            Address = patient.Address,
            PhoneNumber = patient.PhoneNumber
        };

        return dentistDtoAsUser;
    }

    public async Task UpdateAsync(DentistUpdateDto dentistDto, int id)
    {
        var dentist = await _dentistRepository.GetByIdAsync(id, false);

        var patient = await GetDentistFromPatients(dentist);

        Patient patientToUpdate = new()
        {
            Name = dentistDto.Name,
            Surname = dentistDto.Surname,
            Patronymic = dentistDto.Patronymic,
            Email = dentistDto.Email,
            BirthDate = dentistDto.BirthDate,
            PhoneNumber = dentistDto.PhoneNumber,
            Address = dentistDto.Address
        };

        if (!string.IsNullOrWhiteSpace(dentistDto.Password))
        {
            patientToUpdate.PasswordHash = _passwordHasher.Generate(dentistDto.Password);
        }

        await _patientsRepository.UpdateAsync(patient.Id, patientToUpdate);

        Dentist dentistEntity = dentistDto.Adapt<Dentist>();

        var specialization = await _specializationsRepository.GetByNameAsync(dentistDto.Specialization);

        dentistEntity.SpecializationId = specialization.Id;

        await _dentistRepository.UpdateAsync(id, dentistEntity);
    }

    public async Task AddWorkingSchedule(int dentistId, int workingScheduleId)
    {
        Dentist dentist = await GetDentistWithWorkingScheduleAsync(dentistId);

        if (dentist.WorkingSchedule.Count == 5)
        {
            throw new InvalidRequestException($"Dentist with Id:{dentist} have full work week");
        }

        WorkingSchedule workingSchedule = await _workingScheduleRepository.GetById(workingScheduleId);

        if (dentist.WorkingSchedule.Any(x => x.WorkingDay.Equals(workingSchedule.WorkingDay)))
        {
            throw new InvalidRequestException("Dentist already have working day in this day");
        }

        await _dentistRepository.AddWorkingSchedule(dentist, workingSchedule);
    }

    public async Task DeleteWorkingSchedule(int dentistId, int workingScheduleId)
    {
        Dentist dentist = await GetDentistWithWorkingScheduleAsync(dentistId);

        if (dentist.WorkingSchedule.Count == 0)
        {
            throw new InvalidRequestException($"Dentist with Id:{dentist.Id} have don't have empty work schedule");
        }

        WorkingSchedule? workingSchedule = await _workingScheduleRepository.GetById(workingScheduleId);

        await _dentistRepository.DeleteWorkingScheduleAsync(dentist, workingSchedule);
    }

    public async Task<WorkingScheduleDtoToReturn> GetWorkingScheduleAsync(int dentistId)
    {
        Dentist dentist = await GetDentistWithWorkingScheduleAsync(dentistId);

        var workingSchedule = dentist.WorkingSchedule.Adapt<List<WorkingScheduleDto>>()
            .OrderBy(x => DayOfWeekMap[x.WorkingDay]);

        WorkingScheduleDtoToReturn workingScheduleDto = new()
        {
            DentistName = dentist.Name,
            DentistSurname = dentist.Surname,
            DentistPatronymic = dentist.Patronymic,
            WorkingSchedule = workingSchedule,
        };

        return workingScheduleDto;
    }

    private static readonly Dictionary<string, int> DayOfWeekMap = new()
    {
        {"понедельник", 1},
        {"вторник", 2},
        {"среда", 3},
        {"четверг", 4},
        {"пятница", 5},
        {"суббота", 6},
        {"воскресенье", 7}
    };

    private async Task<Dentist> GetDentistWithWorkingScheduleAsync(int dentistId)
    {
        Dentist? dentist = await _dentistRepository
            .GetAll()
            .Include(ws => ws.WorkingSchedule)
            .FirstOrDefaultAsync(x => x.Id == dentistId);

        if (dentist is null)
        {
            throw new NotFoundException($"Dentist with Id:{dentistId} don't exists");
        }

        return dentist;
    }


    public PagedList<AppointmentDto> GetAppointmentsList(QueryParameters query, DateOnly specificDate)
    {
        int dentistId = GetDentistIdFromClaims();

        var appointments = _dentistRepository.GetAppointmentsList(dentistId, query, specificDate);

        List<AppointmentDto> appointmentsDto = [];

        foreach (var appointment in appointments.Items)
        {
            appointmentsDto.Add(new AppointmentDto()
            {
                AppointmentId = appointment.Id,
                DentistName = appointment.Dentist.Name,
                DentistSurname = appointment.Dentist.Surname,
                DentistPatronymic = appointment.Dentist.Patronymic,
                DentistCabinetNumber = appointment.Dentist.CabinetNumber,
                DentistSpecialization = appointment.Dentist.Specialization.Name,
                PatientName = appointment.Patient.Name,
                PatientSurname = appointment.Patient.Surname,
                PatientPatronymic = appointment.Patient.Patronymic,
                AppointmentDate = DateOnly.FromDateTime(appointment.Date),
                AppointmentTime = TimeOnly.FromDateTime(appointment.Date)
            });
        }

        return new PagedList<AppointmentDto>(appointmentsDto, appointments.Page, appointments.PageSize, appointments.TotalCount);
    }

    public int GetDentistIdFromClaims()
    {
        var dentistIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("dentistId");

        if (dentistIdClaim == null)
        {
            throw new UnauthorizedAccessException("Dentist isn't authorized");
        }

        int patientId = Convert.ToInt32(dentistIdClaim.Value);
        return patientId;
    }
}
