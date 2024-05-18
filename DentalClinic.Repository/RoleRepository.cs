using DentalClinic.Models.Entities;
using DentalClinic.Models.Exceptions;
using DentalClinic.Repository.Contracts;

using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Repository;
public class RoleRepository : IRoleRepository
{
    private readonly ClinicDbContext _context;

    public RoleRepository(ClinicDbContext context)
    {
        _context = context;
    }
    public IQueryable<Role> GetAll()
    {
        return _context.Roles.AsQueryable();
    }

    public async Task<Role> GetByName(string name)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == name);

        if (role is null)
        {
            throw new NotFoundException($"Role with provided name:{role} don't exists");
        }

        return role;
    }
}