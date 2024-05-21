using DentalClinic.API.Filters;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.Appointments;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinic.API.Controllers;
[ApiController]
[Route("api/[controller]")]
[ValidateId]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentsService _appointmentsService;
    public AppointmentsController(IAppointmentsService appointmentsService)
    {
        _appointmentsService = appointmentsService;
    }

    [HttpPost("{dentistId:int}/{appointmentId:int}")]
    [Authorize(Roles = "Patient")]
    public async Task<ActionResult<AppointmentDto>> MakeAppointment(int dentistId, int appointmentId)
    {
        var appointment = await _appointmentsService.MakeAppointmentAsync(dentistId, appointmentId);

        return Ok(appointment);
    }

    [HttpGet("{appointmentId:int}/get-appointment")]
    [Authorize(Roles = "Patient")]
    public async Task<ActionResult<AppointmentDto>> GetAppointment(int appointmentId)
    {
        var appointment = await _appointmentsService.GetById(appointmentId);

        return Ok(appointment);
    }

    [HttpDelete("{appointmentId:int}/cancel-appointment")]
    [Authorize(Roles = "Patient")]
    public async Task<ActionResult> CancelAppointment(int appointmentId)
    {
        await _appointmentsService.CancelAppointmentAsync(appointmentId);

        return NoContent();
    }
}
