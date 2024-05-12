using DentalClinic.Models.Entities;
using DentalClinic.Repository;
using DentalClinic.Repository.Contracts;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.Dentists;
using DentalClinic.Shared.Pagination;

using Mapster;

using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Services;
public class DentistsService : IDentistsService
{
    private readonly IDentistRepository _dentistRepository;
    private readonly ISpecializationsRepository _specializationsRepository;

    public DentistsService(IDentistRepository dentistRepository,
                           ISpecializationsRepository specializationsRepository)
    {
        _dentistRepository = dentistRepository;
        _specializationsRepository = specializationsRepository;
    }

    public PagedList<DentistDto> GetPaged(QueryParameters query)
    {
        var dentists = _dentistRepository.GetPaged(query);

        var dentistsDto = dentists.Items.Adapt<List<DentistDto>>();

        return new PagedList<DentistDto>(dentistsDto, dentists.Page, dentists.PageSize, dentists.TotalCount);
    }

    public async Task<DentistDto> CreateAsync(DentistCreateDto dentistDto)
    {
        var specialization = await _specializationsRepository
            .GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Name == dentistDto.Specialization);

        if (specialization == null)
        {
            throw new ArgumentException("Specialization with provided name don't exists in database");
        }

        Dentist dentist = dentistDto.Adapt<Dentist>();

        await _dentistRepository.CreateAsync(dentist, specialization.Id);

        DentistDto dentistToReturn = dentist.Adapt<DentistDto>();

        return dentistToReturn;
    }

    public async Task DeleteAsync(int id)
    {
        if (!IsIdValid(id))
        {
            throw new ArgumentException("Invalid id");
        }

        await _dentistRepository.DeleteAsync(id);
    }

    public async Task UpdateAsync(DentistUpdateDto dentistDto, int id)
    {
        if (!IsIdValid(id))
        {
            throw new ArgumentException("Invalid id");
        }

        Dentist dentistEntity = dentistDto.Adapt<Dentist>();

        var specialization = await _specializationsRepository
            .GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Name == dentistDto.Specialization);

        if (specialization == null)
        {
            throw new ArgumentException("Specialization with provided name don't exists in database");
        }

        dentistEntity.SpecializationId = specialization.Id;

        await _dentistRepository.UpdateAsync(id, dentistEntity);
    }

    private bool IsIdValid(int id)
    {
        return id >= 0 && id <= int.MaxValue;
    }
}
