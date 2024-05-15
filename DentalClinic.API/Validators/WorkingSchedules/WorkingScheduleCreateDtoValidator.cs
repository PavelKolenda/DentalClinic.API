using DentalClinic.Shared.DTOs.WorkingSchedules;

using FluentValidation;

namespace DentalClinic.API.Validators.WorkingSchedules;
public class WorkingScheduleCreateDtoValidator : AbstractValidator<WorkingScheduleCreateDto>
{
    public WorkingScheduleCreateDtoValidator()
    {
        RuleFor(x => x.Start)
            .NotEmpty()
            .LessThan(x => x.End);

        RuleFor(x => x.End)
            .NotEmpty()
            .GreaterThan(x => x.Start);

        RuleFor(x => x.WorkingDay)
            .NotEmpty();
    }
}