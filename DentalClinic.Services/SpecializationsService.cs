using DentalClinic.Models.Entities;
using DentalClinic.Repository.Contracts;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.Specializations;
using DentalClinic.Shared.Pagination;

using Mapster;

namespace DentalClinic.Services;
public class SpecializationsService : ISpecializationsService
{
    private readonly ISpecializationsRepository _specializationsRepository;

    public SpecializationsService(ISpecializationsRepository specializationsRepository)
    {
        _specializationsRepository = specializationsRepository;
    }

    public PagedList<SpecializationDto> GetPaged(QueryParameters query)
    {
        var specializations = _specializationsRepository.GetPaged(query);

        var specializationsDto = specializations.Items.Adapt<List<SpecializationDto>>();

        return new PagedList<SpecializationDto>(specializationsDto, specializations.Page, specializations.PageSize, specializations.TotalCount);
    }

    public async Task<SpecializationDto> CreateAsync(SpecializationCreateDto specializationCreateDto)
    {
        Specialization specialization = specializationCreateDto.Adapt<Specialization>();

        specialization = await _specializationsRepository.CreateAsync(specialization);

        return specialization.Adapt<SpecializationDto>();
    }

    public async Task DeleteAsync(int id)
    {
        await _specializationsRepository.GetByIdAsync(id, false);

        await _specializationsRepository.DeleteAsync(id);
    }

    public async Task UpdateAsync(int id, SpecializationUpdateDto specializationUpdateDto)
    {
        await _specializationsRepository.GetByIdAsync(id, false);

        Specialization specialization = specializationUpdateDto.Adapt<Specialization>();

        await _specializationsRepository.UpdateAsync(id, specialization);
    }

    public async Task<SpecializationDto> GetByIdAsync(int id)
    {
        Specialization specialization = await _specializationsRepository.GetByIdAsync(id, false);

        return specialization.Adapt<SpecializationDto>();
    }
}
