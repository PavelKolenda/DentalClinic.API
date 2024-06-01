using DentalClinic.API.Filters;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.Dentists;
using DentalClinic.Shared.DTOs.Specializations;
using DentalClinic.Shared.Pagination;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinic.API.Controllers;
[ApiController]
[Route("api/[controller]")]
[ValidateId]
[Authorize(Roles = "Admin")]
public class SpecializationsController : ControllerBase
{
    private readonly ISpecializationsService _specializationsService;
    private readonly IDentistsService _dentistsService;
    public SpecializationsController(ISpecializationsService specializationsService, IDentistsService dentistsService)
    {
        _specializationsService = specializationsService;
        _dentistsService = dentistsService;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<PagedList<SpecializationDto>> GetPaged([FromQuery] QueryParameters query)
    {
        var specializations = _specializationsService.GetPaged(query);

        return Ok(specializations);
    }

    [HttpGet("{id:int}", Name = "GetSpecializationById")]
    public async Task<ActionResult<SpecializationDto>> GetById(int id)
    {
        var specialization = await _specializationsService.GetByIdAsync(id);

        return Ok(specialization);
    }

    [HttpPost]
    public async Task<ActionResult<SpecializationDto>> CreateAsync([FromBody] SpecializationCreateDto specializationCreateDto)
    {
        var specialization = await _specializationsService.CreateAsync(specializationCreateDto);

        return CreatedAtRoute(routeName: "GetSpecializationById", routeValues: new { id = specialization.Id }, value: specialization);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] SpecializationUpdateDto specializationUpdateDto)
    {
        await _specializationsService.UpdateAsync(id, specializationUpdateDto);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _specializationsService.DeleteAsync(id);

        return NoContent();
    }

    [HttpGet("{specializationId:int}/dentists")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<DentistDto>>> GetBySpecialization(int specializationId)
    {
        var dentists = await _dentistsService.GetBySpecialization(specializationId);

        return Ok(dentists);
    }
}
