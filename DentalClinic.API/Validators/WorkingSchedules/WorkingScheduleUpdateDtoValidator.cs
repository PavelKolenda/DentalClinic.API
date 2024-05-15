using DentalClinic.Shared.DTOs.WorkingSchedules;

using FluentValidation;

namespace DentalClinic.API.Validators.WorkingSchedules;

public class WorkingScheduleUpdateDtoValidator : AbstractValidator<WorkingScheduleUpdateDto>
{
    public WorkingScheduleUpdateDtoValidator()
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
