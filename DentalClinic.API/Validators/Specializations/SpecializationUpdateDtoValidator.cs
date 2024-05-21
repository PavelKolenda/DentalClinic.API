using DentalClinic.Shared.DTOs.Specializations;

using FluentValidation;

namespace DentalClinic.API.Validators.Specializations;

public class SpecializationUpdateDtoValidator : AbstractValidator<SpecializationUpdateDto>
{
    public SpecializationUpdateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(128);
    }
}
