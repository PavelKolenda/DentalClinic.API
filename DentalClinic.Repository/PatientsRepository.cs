﻿using System.Linq.Expressions;

using DentalClinic.Models.Entities;
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

    public PatientsRepository(ClinicDbContext context, ILogger<PatientsRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IQueryable<Patient> GetAll()
    {
        return _context.Patients.AsQueryable();
    }
    public PagedList<Patient> GetPaged(QueryParameters query)
    {
        var dbQuery = _context.Patients.AsQueryable();

        if (query.SortOrder is not null)
        {
            if (query.SortOrder == Shared.Sorting.SortOrder.Ascending)
            {
                dbQuery.OrderBy(GetSortColumn(query));
            }
            else
            {
                dbQuery.OrderByDescending(GetSortColumn(query));
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
            throw new ArgumentException($"Patient with Id:{id} don't exists");
        }

        return patient;
    }

    public async Task<Patient> CreateAsync(Patient patient)
    {
        Role? role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Patient");

        if (role == null)
        {
            throw new ArgumentException("Role don't exists in database");
        }

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
        await _context.Patients.Where(p => p.Id == id)
            .ExecuteUpdateAsync(s => s
            .SetProperty(p => p.Name, patient.Name)
            .SetProperty(p => p.Surname, patient.Surname)
            .SetProperty(p => p.Patronymic, patient.Patronymic)
            .SetProperty(p => p.BirthDate, patient.BirthDate)
            .SetProperty(p => p.Email, patient.Email)
            .SetProperty(p => p.PasswordHash, patient.PasswordHash)
            );

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated Patient with Id:{id}", id);
    }
}