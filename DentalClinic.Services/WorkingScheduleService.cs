using DentalClinic.Models.Entities;
using DentalClinic.Models.Exceptions;
using DentalClinic.Repository.Contracts;
using DentalClinic.Repository.Contracts.Queries;
using DentalClinic.Services.Contracts;
using DentalClinic.Shared.DTOs.WorkingSchedules;
using DentalClinic.Shared.Pagination;

using Mapster;

using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Services;
public class WorkingScheduleService : IWorkingScheduleService
{
    private readonly IWorkingScheduleRepository _workingScheduleRepository;

    public WorkingScheduleService(IWorkingScheduleRepository workingScheduleService)
    {
        _workingScheduleRepository = workingScheduleService;
    }

    public PagedList<WorkingScheduleDto> GetPaged(QueryParameters query, string? dayFilter)
    {
        var wsPaged = _workingScheduleRepository.GetPaged(query, dayFilter);

        var wsDto = wsPaged.Items.Adapt<List<WorkingScheduleDto>>();

        return new PagedList<WorkingScheduleDto>(wsDto, wsPaged.Page, wsPaged.PageSize, wsPaged.TotalCount);
    }

    public async Task<WorkingScheduleDto> GetByIdAsync(int id)
    {
        WorkingSchedule workingSchedule = await _workingScheduleRepository.GetById(id);

        var workingScheduleDto = workingSchedule.Adapt<WorkingScheduleDto>();

        return workingScheduleDto;
    }

    public async Task<WorkingScheduleDto> CreateAsync(WorkingScheduleCreateDto workingScheduleCreateDto)
    {
        if (!IsWorkingDayValid(workingScheduleCreateDto.WorkingDay))
        {
            throw new InvalidRequestException("Invalid working day");
        }

        var isWorkingExists = await _workingScheduleRepository.GetAll()
            .FirstOrDefaultAsync(x => x.Start == workingScheduleCreateDto.Start
            && x.End == workingScheduleCreateDto.End
            && x.WorkingDay == workingScheduleCreateDto.WorkingDay);

        if (isWorkingExists != null)
        {
            throw new InvalidRequestException("Provided working schedule already exists");
        }

        if ((workingScheduleCreateDto.End - workingScheduleCreateDto.Start).TotalHours > 12)
        {
            throw new InvalidRequestException("Working day can't be more than 12 hours");
        }

        WorkingSchedule workingSchedule = workingScheduleCreateDto.Adapt<WorkingSchedule>();

        var wsToReturn = await _workingScheduleRepository.CreateAsync(workingSchedule);

        var wsDto = wsToReturn.Adapt<WorkingScheduleDto>();
        return wsDto;
    }

    public async Task UpdateAsync(int id, WorkingScheduleUpdateDto wsUpdateDto)
    {
        if (!IsWorkingDayValid(wsUpdateDto.WorkingDay))
        {
            throw new InvalidRequestException("Invalid working day");
        }

        if ((wsUpdateDto.End - wsUpdateDto.Start).TotalHours > 12)
        {
            throw new InvalidRequestException("Working day can't be more than 12 hours");
        }

        var workingSchedule = wsUpdateDto.Adapt<WorkingSchedule>();

        await _workingScheduleRepository.UpdateAsync(id, workingSchedule);
    }

    public async Task DeleteAsync(int id)
    {
        await _workingScheduleRepository.DeleteAsync(id);
    }

    private bool IsWorkingDayValid(string workingDay)
    {
        return workingDay.ToLowerInvariant()
            switch
        {
            "понедельник" => true,
            "вторник" => true,
            "среда" => true,
            "четверг" => true,
            "пятница" => true,
            _ => false
        };
    }
}
