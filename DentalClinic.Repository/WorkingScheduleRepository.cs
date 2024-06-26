﻿using DentalClinic.Models.Entities;
using DentalClinic.Models.Exceptions;
using DentalClinic.Repository.Contracts;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.Pagination;

using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Repository;
public class WorkingScheduleRepository : IWorkingScheduleRepository
{
    private readonly ClinicDbContext _context;

    public WorkingScheduleRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public IQueryable<WorkingSchedule> GetAll()
    {
        return _context.WorkingSchedules.AsQueryable();
    }

    public PagedList<WorkingSchedule> GetPaged(QueryParameters query, string? dayFilter)
    {
        var dbQuery = _context.WorkingSchedules.AsQueryable();

        if (!string.IsNullOrWhiteSpace(dayFilter))
        {
            dbQuery = dbQuery.Where(x => x.WorkingDay == dayFilter);
        }

        return PagedListExtensions<WorkingSchedule>.Create(dbQuery, query.Page, query.PageSize);
    }

    public async Task<WorkingSchedule> CreateAsync(WorkingSchedule workingSchedule)
    {
        await _context.WorkingSchedules.AddAsync(workingSchedule);
        await _context.SaveChangesAsync();

        return workingSchedule;
    }

    public async Task DeleteAsync(int id)
    {
        await _context.WorkingSchedules.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task<WorkingSchedule> GetById(int id)
    {
        WorkingSchedule? workingSchedule = await _context.WorkingSchedules.FirstOrDefaultAsync(x => x.Id == id);

        if (workingSchedule is null)
        {
            throw new NotFoundException($"Working schedule with Id:{id} don't exists");
        }

        return workingSchedule;
    }

    public async Task UpdateAsync(int id, WorkingSchedule workingSchedule)
    {
        await _context.WorkingSchedules.Where(x => x.Id == id)
            .ExecuteUpdateAsync(p => p
            .SetProperty(p => p.Start, workingSchedule.Start)
            .SetProperty(p => p.End, workingSchedule.End)
            .SetProperty(p => p.WorkingDay, workingSchedule.WorkingDay));

        await _context.SaveChangesAsync();
    }
}