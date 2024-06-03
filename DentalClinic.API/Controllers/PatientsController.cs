using DentalClinic.API.Filters;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.Appointments;
using DentalClinic.Shared.DTOs.Patients;
using DentalClinic.Shared.DTOs.Roles;
using DentalClinic.Shared.Pagination;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinic.API.Controllers;
[ApiController]
[Route("api/[controller]")]
[ValidateId]
public class PatientsController : ControllerBase
{
    private readonly IPatientsService _patientsService;
    public PatientsController(IPatientsService patientsService)
    {
        _patientsService = patientsService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public ActionResult<PagedList<PatientDto>> GetPaged([FromQuery] QueryParameters query)
    {
        PagedList<PatientDto> patients = _patientsService.GetPaged(query);

        return Ok(patients);
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<PatientDto>> GetByIdAsync(int id)
    {
        PatientDto patient = await _patientsService.GetByIdAsync(id);

        return Ok(patient);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PatientDto>> DeleteAsync(int id)
    {
        await _patientsService.DeleteAsync(id);

        return NoContent();
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "Patient")]
    public async Task<ActionResult<PatientUpdateDto>> UpdateAsync(int id, [FromBody] PatientUpdateDto patientUpdateDto)
    {
        await _patientsService.UpdateAsync(patientUpdateDto, id);

        return NoContent();
    }

    [HttpPost("{id:int}/update-roles")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateRoles(int id, [FromBody] RoleDto roleDto)
    {
        await _patientsService.UpdateRoles(id, roleDto);
        return NoContent();
    }

    [HttpGet("appointments")]
    [Authorize(Roles = "Patient")]
    public ActionResult<PagedList<AppointmentDto>> GetAppointments([FromQuery] QueryParameters query)
    {
        var appointments = _patientsService.GetAppointments(query);

        return Ok(appointments);
    }
}
