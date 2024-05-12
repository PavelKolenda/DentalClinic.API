using System.Linq.Expressions;

using DentalClinic.Models.Entities;
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

    public DentistRepository(ClinicDbContext context, ILogger<DentistRepository> logger)
    {
        _context = context;
        _logger = logger;
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

    public async Task DeleteAsync(int id)
    {
        await _context.Dentists.Where(p => p.Id == id).ExecuteDeleteAsync();
        _logger.LogInformation("Delete Dentist with Id:{id}", id);
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
