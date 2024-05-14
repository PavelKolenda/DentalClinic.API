using DentalClinic.Models.Entities;
using DentalClinic.Repository.Contracts;

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
}