using DentalClinic.Models.Entities;
using DentalClinic.Repository;
using DentalClinic.Repository.Contracts;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.Dentists;
using DentalClinic.Shared.DTOs.WorkingSchedules;
using DentalClinic.Shared.Pagination;

using Mapster;

using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Services;
public class DentistsService : IDentistsService
{
    private readonly IDentistRepository _dentistRepository;
    private readonly ISpecializationsRepository _specializationsRepository;
    private readonly IWorkingScheduleRepository _workingScheduleRepository;

    public DentistsService(IDentistRepository dentistRepository,
                           ISpecializationsRepository specializationsRepository,
                           IWorkingScheduleRepository workingScheduleRepository)
    {
        _dentistRepository = dentistRepository;
        _specializationsRepository = specializationsRepository;
        _workingScheduleRepository = workingScheduleRepository;
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
        await _dentistRepository.DeleteAsync(id);
    }

    public async Task UpdateAsync(DentistUpdateDto dentistDto, int id)
    {
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

    public async Task AddWorkingSchedule(int dentistId, int workingScheduleId)
    {
        Dentist dentist = await GetDentistWithWorkingScheduleAsync(dentistId);

        if (dentist.WorkingSchedule.Count == 5)
        {
            throw new ArgumentException("");
        }

        WorkingSchedule? workingSchedule = await _workingScheduleRepository.GetById(workingScheduleId);

        if (workingSchedule is null)
        {
            throw new ArgumentException("");
        }

        await _dentistRepository.AddWorkingSchedule(dentist, workingSchedule);
    }

    public async Task DeleteWorkingSchedule(int dentistId, int workingScheduleId)
    {
        Dentist dentist = await GetDentistWithWorkingScheduleAsync(dentistId);

        if (dentist.WorkingSchedule.Count == 0)
        {
            throw new ArgumentException("");
        }

        WorkingSchedule? workingSchedule = await _workingScheduleRepository.GetById(workingScheduleId);

        if (workingSchedule is null)
        {
            throw new ArgumentException("");
        }

        await _dentistRepository.DeleteWorkingScheduleAsync(dentist, workingSchedule);
    }

    public async Task<IEnumerable<WorkingScheduleDto>> GetWorkingScheduleAsync(int dentistId)
    {
        Dentist dentist = await GetDentistWithWorkingScheduleAsync(dentistId);

        var workingSchedule = dentist.WorkingSchedule.Adapt<List<WorkingScheduleDto>>()
            .OrderBy(x => DayOfWeekMap[x.WorkingDay]);

        return workingSchedule;
    }

    private static readonly Dictionary<string, int> DayOfWeekMap = new()
    {
         {"понедельник", 1},
         {"вторник", 2},
         {"среда", 3},
         {"четверг", 4},
         {"пятница", 5},
    };

    private async Task<Dentist> GetDentistWithWorkingScheduleAsync(int dentistId)
    {
        Dentist? dentist = await _dentistRepository
            .GetAll()
            .Include(ws => ws.WorkingSchedule)
            .FirstOrDefaultAsync(x => x.Id == dentistId);

        if (dentist is null)
        {
            throw new ArgumentException("");
        }

        return dentist;
    }
}
