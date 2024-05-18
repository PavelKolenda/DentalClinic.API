using DentalClinic.Models.Entities;

namespace DentalClinic.Repository.Contracts
{
    public interface IRoleRepository
    {
        IQueryable<Role> GetAll();
        Task<Role> GetByName(string name);
    }
}