using DentalClinic.Models.Entities;

namespace DentalClinic.Repository.Contracts;
public interface ISpecializationsRepository
{
    IQueryable<Specialization> GetAll();
    Task<Specialization> GetByName(string name);
}
