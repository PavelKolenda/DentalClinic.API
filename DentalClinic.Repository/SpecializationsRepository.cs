using DentalClinic.Models.Entities;
using DentalClinic.Models.Exceptions;
using DentalClinic.Repository.Contracts;

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

    public Task<Specialization> GetByName(string name)
    {
        var specialization = _context.Specializations.FirstOrDefaultAsync(x => x.Name == name);

        if (specialization is null)
        {
            throw new NotFoundException($"Specialization with provided name:{name} don't exists");
        }

        return specialization;
    }
}
