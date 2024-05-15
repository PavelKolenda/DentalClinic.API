using DentalClinic.API.Filters;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.WorkingSchedules;
using DentalClinic.Shared.Pagination;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinic.API.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
[ValidateId]
public class WorkingSchedulesController : ControllerBase
{
    private readonly IWorkingScheduleService _workingScheduleService;

    public WorkingSchedulesController(IWorkingScheduleService workingScheduleService)
    {
        _workingScheduleService = workingScheduleService;
    }

    [HttpGet]
    public ActionResult<PagedList<WorkingScheduleDto>> GetPaged([FromQuery] QueryParameters query)
    {
        var workingSchedules = _workingScheduleService.GetPaged(query);

        return Ok(workingSchedules);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkingScheduleDto>> GetByIdAsync(int id)
    {
        WorkingScheduleDto workingSchedule = await _workingScheduleService.GetByIdAsync(id);

        return Ok(workingSchedule);
    }

    [HttpPost]
    public async Task<ActionResult<WorkingScheduleDto>> CreateAsync([FromBody] WorkingScheduleCreateDto workingSchedule)
    {
        var ws = await _workingScheduleService.CreateAsync(workingSchedule);

        return Ok(ws);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] WorkingScheduleUpdateDto wsCreateDto)
    {
        await _workingScheduleService.UpdateAsync(id, wsCreateDto);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _workingScheduleService.DeleteAsync(id);

        return NoContent();
    }
}