using DentalClinic.Shared.DTOs.Dentists;

using FluentValidation;

namespace DentalClinic.API.Validators.Dentists;

public class DentistUpdateDtoValidator : AbstractValidator<DentistUpdateDto>
{
    public DentistUpdateDtoValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(2)
            .MaximumLength(75)
            .NotEmpty();

        RuleFor(x => x.Surname)
            .MinimumLength(2)
            .MaximumLength(75)
            .NotEmpty();

        RuleFor(x => x.CabinetNumber)
            .InclusiveBetween(1, 25)
            .NotEmpty();

        RuleFor(x => x.Specialization)
            .NotEmpty();
    }
}
