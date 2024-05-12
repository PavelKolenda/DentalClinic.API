using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.Dentists;

using Microsoft.AspNetCore.Mvc;

namespace DentalClinic.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DentistsController : ControllerBase
{
    private readonly IDentistsService _dentistsService;

    public DentistsController(IDentistsService dentistsService)
    {
        _dentistsService = dentistsService;
    }

    [HttpGet]
    public ActionResult GetPaged([FromQuery] QueryParameters query)
    {
        var dentists = _dentistsService.GetPaged(query);

        return Ok(dentists);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] DentistCreateDto dentistCreateDto)
    {
        var dentist = await _dentistsService.CreateAsync(dentistCreateDto);

        return CreatedAtRoute(dentist, dentist.Id);
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
}
