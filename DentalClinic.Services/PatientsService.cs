﻿using DentalClinic.Models.Entities;
using DentalClinic.Repository.Contracts;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.Patients;
using DentalClinic.Shared.Pagination;

using Mapster;

using Microsoft.Extensions.Logging;

namespace DentalClinic.Services;
public class PatientsService : IPatientsService
{
    private readonly IPatientsRepository _patientsRepository;
    private readonly ILogger<PatientsService> _logger;
    public PatientsService(IPatientsRepository patientsRepository, ILogger<PatientsService> logger)
    {
        _patientsRepository = patientsRepository;
        _logger = logger;
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

        await _patientsRepository.UpdateAsync(id, toUpdate);
    }
}