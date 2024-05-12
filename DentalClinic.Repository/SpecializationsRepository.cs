using DentalClinic.Models.Entities;

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
}
