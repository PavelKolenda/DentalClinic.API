using DentalClinic.Models.Entities;
using DentalClinic.Repository.Contracts;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.News;
using DentalClinic.Shared.Pagination;

using Mapster;

namespace DentalClinic.Services;
public class NewsService : INewsService
{
    private readonly INewsRepository _newsRepository;

    public NewsService(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public PagedList<NewsDto> GetPagedList(QueryParameters query)
    {
        var news = _newsRepository.GetPaged(query);
        var newsDto = news.Items.Adapt<List<NewsDto>>();

        return new PagedList<NewsDto>(newsDto, news.Page, news.PageSize, news.TotalCount);
    }

    public async Task<NewsDto> GetByIdAsync(int id)
    {
        var news = await _newsRepository.GetById(id);

        var newsDto = news.Adapt<NewsDto>();

        return newsDto;
    }

    public async Task<NewsDto> CreateAsync(NewsCreateDto newsDto)
    {
        var news = newsDto.Adapt<News>();

        var createdNews = await _newsRepository.CreateAsync(news);

        NewsDto newsToReturnDto = createdNews.Adapt<NewsDto>();

        return newsToReturnDto;
    }

    public async Task DeleteAsync(int id)
    {
        await _newsRepository.DeleteAsync(id);
    }

    public async Task UpdateAsync(int id, NewsUpdateDto newsDto)
    {
        var news = newsDto.Adapt<News>();

        await _newsRepository.UpdateAsync(id, news);
    }
}
