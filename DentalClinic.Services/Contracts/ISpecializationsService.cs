using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Shared.DTOs.Specializations;
using DentalClinic.Shared.Pagination;

namespace DentalClinic.Services.Contracts;
public interface ISpecializationsService
{
    Task<SpecializationDto> CreateAsync(SpecializationCreateDto specializationCreateDto);
    Task DeleteAsync(int id);
    Task<SpecializationDto> GetByIdAsync(int id);
    PagedList<SpecializationDto> GetPaged(QueryParameters query);
    Task UpdateAsync(int id, SpecializationUpdateDto specializationUpdateDto);
}
