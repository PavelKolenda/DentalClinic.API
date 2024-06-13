using DentalClinic.Models.Entities;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.Pagination;

namespace DentalClinic.Repository.Contracts;
public interface INotificationsRepository
{
    Task<Notification> CreateAsync(Notification notification);
    PagedList<Notification> GetPaged(int patientId, QueryParameters query);
}
