using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.Notifications;
using DentalClinic.Shared.Pagination;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinic.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationsService _notificationsService;
    public NotificationsController(INotificationsService notificationsService)
    {
        _notificationsService = notificationsService;
    }

    [HttpGet]
    [Authorize(Roles = "Patient")]
    public PagedList<NotificationDto> Get([FromQuery] QueryParameters query)
    {
        var notifications = _notificationsService.GetPaged(query);

        return notifications;
    }
}
