using DentalClinic.Models.Entities;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs;
using DentalClinic.Shared.DTOs.Appointments;
using DentalClinic.Shared.DTOs.Dentists;
using DentalClinic.Shared.Pagination;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinic.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DentistsController : ControllerBase
{
    private readonly IDentistsService _dentistsService;
    private readonly IAppointmentsService _appointmentsService;

    public DentistsController(IDentistsService dentistsService,
                              IAppointmentsService appointmentsService)
    {
        _dentistsService = dentistsService;
        _appointmentsService = appointmentsService;
    }

    [HttpGet]
    public ActionResult GetPaged([FromQuery] QueryParameters query)
    {
        var dentists = _dentistsService.GetPaged(query);

        return Ok(dentists);
    }


    [HttpGet("{dentistId:int}/available-appointments")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<AvailableAppointmentsDto>>> GetAvailableForMonth(int dentistId)
    {
        var appointments = await _appointmentsService.GetAvailableForMonthAsync(dentistId);

        return Ok(appointments);
    }

    [HttpPost]
    public async Task<ActionResult<DentistDto>> Create([FromBody] DentistCreateDto dentistCreateDto)
    {
        var dentist = await _dentistsService.CreateAsync(dentistCreateDto);

        return CreatedAtRoute(new { id = dentist.Id }, dentist);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] DentistUpdateDto dentistUpdateDto)
    {
        await _dentistsService.UpdateAsync(dentistUpdateDto, id);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _dentistsService.DeleteAsync(id);

        return NoContent();
    }

    [HttpGet("{id:int}/schedule")]
    public async Task<ActionResult<IEnumerable<WorkingSchedule>>> GetSchedule(int id)
    {
        var schedule = await _dentistsService.GetWorkingScheduleAsync(id);

        return Ok(schedule);
    }

    [HttpPost("{id:int}/schedule/{scheduleId:int}")]
    public async Task<ActionResult> AddSchedule(int id, int scheduleId)
    {
        await _dentistsService.AddWorkingSchedule(id, scheduleId);
        return NoContent();
    }

    [HttpDelete("{id:int}/schedule/{scheduleId:int}")]
    public async Task<ActionResult> DeleteSchedule(int id, int scheduleId)
    {
        await _dentistsService.DeleteWorkingSchedule(id, scheduleId);

        return NoContent();
    }

    [HttpGet("appointments-list")]
    [Authorize(Roles = "Dentist")]
    public ActionResult<PagedList<AppointmentDto>> GetAppointmentsList([FromQuery] QueryParameters queryParameters,
        [FromQuery] DateOnly specificDate)
    {
        var appointments = _dentistsService.GetAppointmentsList(queryParameters, specificDate);

        return Ok(appointments);
    }
}
