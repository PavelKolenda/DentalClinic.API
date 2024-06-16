using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.DTOs.Notifications;
using DentalClinic.Shared.Pagination;

namespace DentalClinic.Services.Contracts;

public interface INotificationsService
{
    PagedList<NotificationDto> GetPaged(QueryParameters query);
}
