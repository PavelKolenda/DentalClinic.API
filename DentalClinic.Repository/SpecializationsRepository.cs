using System.Linq.Expressions;

using DentalClinic.Models.Entities;
using DentalClinic.Models.Exceptions;
using DentalClinic.Repository.Contracts;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.Pagination;

using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Repository;
public class SpecializationsRepository : ISpecializationsRepository
{
    private readonly ClinicDbContext _context;

    public SpecializationsRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public IQueryable<Specialization> GetAll()
    {
        return _context.Specializations.AsQueryable();
    }

    public async Task<Specialization> CreateAsync(Specialization specialization)
    {
        await _context.Specializations.AddAsync(specialization);
        await _context.SaveChangesAsync();

        return specialization;
    }

    public async Task<Specialization> GetByIdAsync(int id, bool trackChanges)
    {
        IQueryable<Specialization> query = _context.Specializations;

        if (!trackChanges)
        {
            query.AsNoTracking();
        }

        Specialization? specialization = await query.FirstOrDefaultAsync(x => x.Id == id);

        if (specialization is null)
        {
            throw new NotFoundException($"Specialization with Id:{id} don't exists");
        }

        return specialization;
    }

    public async Task DeleteAsync(int id)
    {
        await _context.Specializations.Where(p => p.Id == id).ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(int id, Specialization specialization)
    {
        await _context.Specializations.Where(s => s.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(s => s.Name, specialization.Name)
            );

        await _context.SaveChangesAsync();
    }

    public Task<Specialization> GetByName(string name)
    {
        var specialization = _context.Specializations.FirstOrDefaultAsync(x => x.Name == name);

        if (specialization is null)
        {
            throw new NotFoundException($"Specialization with provided name:{name} don't exists");
        }

        return specialization;
    }
    public PagedList<Specialization> GetPaged(QueryParameters query)
    {
        var dbQuery = _context.Specializations.AsQueryable().AsNoTracking();

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

        return PagedListExtensions<Specialization>.Create(dbQuery, query.Page, query.PageSize);
    }
    private static Expression<Func<Specialization, object>> GetSortColumn(QueryParameters query)
    {
        return query.SortColumn?.ToLowerInvariant()
            switch
        {
            _ => p => p.Name
        };
    }
}
