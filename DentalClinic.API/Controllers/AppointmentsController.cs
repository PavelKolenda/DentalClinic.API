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
    private readonly IAppointmentInfoDownload _appointmentInfoDownload;
    public AppointmentsController(IAppointmentsService appointmentsService, IAppointmentInfoDownload appointmentInfoDownload)
    {
        _appointmentsService = appointmentsService;
        _appointmentInfoDownload = appointmentInfoDownload;
    }

    [HttpPost("{dentistId:int}/{appointmentId:int}")]
    [Authorize(Roles = "Patient")]
    public async Task<ActionResult<AppointmentDto>> MakeAppointment(int dentistId, int appointmentId)
    {
        var appointment = await _appointmentsService.MakeAppointmentAsync(dentistId, appointmentId);

        return Ok(appointment);
    }

    [HttpPost("{patientId:int}/{appointmentId:int}/reenrollment")]
    [Authorize(Roles = "Dentist")]
    public async Task<ActionResult<AppointmentDto>> PatientReenrollment(int patientId, int appointmentId)
    {
        var appointment = await _appointmentsService.PatientReenrollment(patientId, appointmentId);

        return Ok(appointment);
    }

    [HttpGet("{appointmentId:int}")]
    [Authorize(Roles = "Patient")]
    public async Task<ActionResult<AppointmentDto>> GetAppointment(int appointmentId)
    {
        var appointment = await _appointmentsService.GetById(appointmentId);

        return Ok(appointment);
    }

    [HttpDelete("{appointmentId:int}")]
    [Authorize(Roles = "Patient")]
    public async Task<ActionResult> CancelAppointment(int appointmentId)
    {
        await _appointmentsService.CancelAppointmentAsync(appointmentId);

        return NoContent();
    }

    [HttpGet("{appointmentId:int}/download")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> DownloadAppointmentInfo(int appointmentId)
    {
        var pdfBytes = await _appointmentInfoDownload.Download(appointmentId);

        return File(pdfBytes, "application/pdf", "appointment.pdf");
    }
}
