using DentalClinic.Models.Entities;
using DentalClinic.Models.Exceptions;
using DentalClinic.Repository.Contracts;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Services.Auth;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.Patients;
using DentalClinic.Shared.DTOs.Roles;
using DentalClinic.Shared.Pagination;

using Mapster;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DentalClinic.Services;
public class PatientsService : IPatientsService
{
    private readonly IPatientsRepository _patientsRepository;
    private readonly ILogger<PatientsService> _logger;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRoleRepository _roleRepository;
    public PatientsService(IPatientsRepository patientsRepository,
                           ILogger<PatientsService> logger,
                           IPasswordHasher passwordHasher,
                           IRoleRepository roleRepository)
    {
        _patientsRepository = patientsRepository;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _roleRepository = roleRepository;
    }

    public async Task<PatientDto> GetByIdAsync(int id)
    {
        Patient patient = await _patientsRepository.GetById(id, false);
        PatientDto patientDto = patient.Adapt<PatientDto>();

        return patientDto;
    }

    public async Task DeleteAsync(int id)
    {
        await _patientsRepository.DeleteAsync(id);
    }

    public PagedList<PatientDto> GetPaged(QueryParameters query)
    {
        PagedList<Patient> patientPagedList = _patientsRepository.GetPaged(query);

        var patientsDto = patientPagedList.Items.Adapt<List<PatientDto>>();

        return new PagedList<PatientDto>(patientsDto, patientPagedList.Page, patientPagedList.PageSize, patientPagedList.TotalCount);
    }

    public async Task UpdateAsync(PatientUpdateDto patientUpdateDto, int id)
    {
        Patient toUpdate = patientUpdateDto.Adapt<Patient>();

        toUpdate.PasswordHash = _passwordHasher.Generate(patientUpdateDto.Password);

        await _patientsRepository.UpdateAsync(id, toUpdate);

        _logger.LogInformation("Update patient with Id:{id}", id);
    }

    public async Task UpdateRoles(int id, RoleDto roleDto)
    {
        Patient? patient = await _patientsRepository.GetAll().Include(r => r.Roles).FirstOrDefaultAsync(p => p.Id == id);

        if (patient == null)
        {
            throw new NotFoundException($"Patient with provided Id:{id} don't exists");
        }

        List<Role> roles = [];

        foreach (string role in roleDto.Roles)
        {
            roles.Add(await _roleRepository.GetByName(role));
        }

        await _patientsRepository.UpdateRoles(patient, roles);
        _logger.LogInformation("Update role for user with Id:{id}", id);
    }
}
