using DentalClinic.API.Filters;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.News;
using DentalClinic.Shared.Pagination;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinic.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ValidateId]
public class NewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet]
    public ActionResult<PagedList<NewsDto>> GetPaged([FromQuery] QueryParameters query)
    {
        var pagedList = _newsService.GetPagedList(query);

        return Ok(pagedList);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<NewsDto>> GetById(int id)
    {
        var news = await _newsService.GetByIdAsync(id);

        return Ok(news);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<NewsDto>> Create([FromBody] NewsCreateDto newsCreateDto)
    {
        var news = await _newsService.CreateAsync(newsCreateDto);

        return Ok(news);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _newsService.DeleteAsync(id);

        return NoContent();
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateAsync(int id, NewsUpdateDto newsDto)
    {
        await _newsService.UpdateAsync(id, newsDto);

        return NoContent();
    }
}
