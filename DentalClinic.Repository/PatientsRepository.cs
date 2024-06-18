using System.Linq.Expressions;

using DentalClinic.Models.Entities;
using DentalClinic.Models.Exceptions;
using DentalClinic.Repository.Contracts;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.Pagination;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DentalClinic.Repository;
public class PatientsRepository : IPatientsRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<PatientsRepository> _logger;
    private readonly IRoleRepository _roleRepository;

    public PatientsRepository(ClinicDbContext context,
                              ILogger<PatientsRepository> logger,
                              IRoleRepository roleRepository)
    {
        _context = context;
        _logger = logger;
        _roleRepository = roleRepository;
    }

    public IQueryable<Patient> GetAll()
    {
        return _context.Patients.AsQueryable();
    }
    public PagedList<Patient> GetPaged(QueryParameters query)
    {
        var dbQuery = _context.Patients.AsQueryable()
            .Include(r => r.Roles)
            .Where(x => x.Roles.Count == 1 && x.Roles.Any(r => r.Name == "Patient"));

        if (query.SortOrder is not null)
        {
            if (query.SortOrder == Shared.Sorting.SortOrder.Ascending)
            {
                dbQuery = dbQuery.OrderBy(GetSortColumn(query));
            }
            else
            {
                dbQuery = dbQuery.OrderByDescending(GetSortColumn(query));
            }
        }

        return PagedListExtensions<Patient>.Create(dbQuery, query.Page, query.PageSize);
    }
    private static Expression<Func<Patient, object>> GetSortColumn(QueryParameters query)
    {
        return query.SortColumn?.ToLowerInvariant()
            switch
        {
            "name" => p => p.Name,
            "surname" => p => p.Surname,
            "patronymic" => p => p.Patronymic,
            "birthdate" => p => p.BirthDate,
            _ => p => p.Name
        };
    }

    public async Task<Patient> GetById(int id, bool trackChanges)
    {
        Patient? patient = trackChanges
            ? await _context.Patients.FirstOrDefaultAsync(p => p.Id == id)
            : await _context.Patients.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        if (patient is null)
        {
            throw new NotFoundException($"Patient with Id:{id} don't exists");
        }

        return patient;
    }

    public async Task UpdateRoles(Patient patient, IEnumerable<Role> roles)
    {
        foreach (Role role in roles)
        {
            patient.Roles.Add(role);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<Patient> CreateAsync(Patient patient)
    {
        Role? role = await _roleRepository.GetByName("Patient");

        patient.Roles = [role];

        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Create Patient with Id:{Id}", patient.Id);
        return patient;
    }

    public async Task<Patient> CreateAsync(Patient patient, Role role)
    {
        patient.Roles = [role];
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Create Patient with Id:{Id}", patient.Id);
        return patient;
    }

    public async Task DeleteAsync(int id)
    {
        await _context.Patients.Where(p => p.Id == id).ExecuteDeleteAsync();
        _logger.LogInformation("Delete Patient with Id:{id}", id);
    }

    public async Task UpdateAsync(int id, Patient patient)
    {
        var patientEntity = await _context.Patients.FirstOrDefaultAsync(x => x.Id == id);

        patientEntity.Name = patient.Name;
        patientEntity.Surname = patient.Surname;
        patientEntity.Patronymic = patient.Patronymic;
        patientEntity.Address = patient.Address;
        patientEntity.Email = patient.Email;
        patientEntity.PhoneNumber = patient.PhoneNumber;
        patientEntity.BirthDate = patient.BirthDate;

        if (!string.IsNullOrEmpty(patient.PasswordHash) && patient.PasswordHash != null)
        {
            patientEntity.PasswordHash = patient.PasswordHash;
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated Patient with Id:{id}", id);
    }

    public PagedList<Appointment> GetAllAppointments(int patientId, QueryParameters query)
    {
        var appointments = _context.Appointments
            .AsNoTracking()
            .AsQueryable()
            .AsSingleQuery()
            .Include(x => x.Patient)
            .Include(d => d.Dentist)
                .ThenInclude(s => s.Specialization)
            .Where(x => x.PatientId == patientId)
            .OrderBy(x => x.Date);


        return PagedListExtensions<Appointment>.Create(appointments, query.Page, query.PageSize);
    }
}