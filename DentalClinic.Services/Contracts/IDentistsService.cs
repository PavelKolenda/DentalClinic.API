using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.DTOs.Dentists;
using DentalClinic.Shared.Pagination;

namespace DentalClinic.Services.Contracts;
public interface IDentistsService
{
    Task<DentistDto> CreateAsync(DentistCreateDto dentistDto);
    Task DeleteAsync(int id);
    PagedList<DentistDto> GetPaged(QueryParameters query);
    Task UpdateAsync(DentistUpdateDto dentistDto, int id);
}
