using DentalClinic.Models.Entities;

namespace DentalClinic.Repository
{
    public interface ISpecializationsRepository
    {
        IQueryable<Specialization> GetAll();
    }
}