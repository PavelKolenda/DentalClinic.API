using DentalClinic.Repository.Contracts;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.Notifications;
using DentalClinic.Shared.Pagination;

using Mapster;

using Microsoft.AspNetCore.Http;

namespace DentalClinic.Services;
public class NotificationsService : INotificationsService
{
    private readonly INotificationsRepository _notificationsRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NotificationsService(INotificationsRepository notificationsRepository, IHttpContextAccessor httpContextAccessor)
    {
        _notificationsRepository = notificationsRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public PagedList<NotificationDto> GetPaged(QueryParameters query)
    {
        int patientId = GetPatientIdFromClaims();

        var notifications = _notificationsRepository.GetPaged(patientId, query);

        var notificationsDto = notifications.Items.Adapt<List<NotificationDto>>();

        return new PagedList<NotificationDto>(notificationsDto, notifications.Page, notifications.PageSize, notifications.TotalCount);
    }

    public int GetPatientIdFromClaims()
    {
        var patientIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("Id");

        if (patientIdClaim == null)
        {
            throw new UnauthorizedAccessException("Patient isn't authorized");
        }

        int patientId = Convert.ToInt32(patientIdClaim.Value);
        return patientId;
    }
}
