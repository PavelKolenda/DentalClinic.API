using DentalClinic.Models.Entities;
using DentalClinic.Models.Exceptions;
using DentalClinic.Repository.Contracts;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.Pagination;
using DentalClinic.Shared.Sorting;

using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Repository;
public class NewsRepository : INewsRepository
{
    private readonly ClinicDbContext _context;

    public NewsRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public PagedList<News> GetPaged(QueryParameters query)
    {
        var dbQuery = _context.News
            .AsQueryable()
            .OrderBy(x => x.CreatedAt);

        if (query.SortOrder is not null)
        {
            if (query.SortOrder == SortOrder.Ascending)
            {
                dbQuery = dbQuery.OrderBy(x => x.CreatedAt);
            }
            else
            {
                dbQuery = dbQuery.OrderByDescending(x => x.CreatedAt);
            }
        }

        return PagedListExtensions<News>.Create(dbQuery, query.Page, query.PageSize);
    }

    public async Task<News> GetById(int id)
    {
        var news = await _context.News.FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"News with id:{id} not found");

        return news;
    }

    public async Task<News> CreateAsync(News news)
    {
        await _context.News.AddAsync(news);
        await _context.SaveChangesAsync();

        return news;
    }

    public async Task DeleteAsync(int id)
    {
        await _context.News.Where(p => p.Id == id).ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(int id, News news)
    {
        await _context.News.Where(d => d.Id == id)
            .ExecuteUpdateAsync(s => s
            .SetProperty(p => p.Title, news.Title)
            .SetProperty(p => p.Text, news.Text)
            .SetProperty(p => p.CreatedAt, news.CreatedAt)
            );

        await _context.SaveChangesAsync();
    }
}