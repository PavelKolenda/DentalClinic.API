using DentalClinic.Models.Entities;
using DentalClinic.Repository.Contracts;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.Pagination;

namespace DentalClinic.Repository;
public class NotificationsRepository : INotificationsRepository
{
    private readonly ClinicDbContext _context;

    public NotificationsRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public PagedList<Notification> GetPaged(int patientId, QueryParameters query)
    {
        var dbQuery = _context.Notifications.AsQueryable()
            .Where(x => x.PatientId == patientId)
            .OrderBy(x => x.SandedAt);

        return PagedListExtensions<Notification>.Create(dbQuery, query.Page, query.PageSize);
    }

    public async Task<Notification> CreateAsync(Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();

        return notification;
    }
}
