using DentalClinic.Shared.DTOs.Specializations;

using FluentValidation;

namespace DentalClinic.API.Validators.Specializations;
public class SpecializationCreateDtoValidator : AbstractValidator<SpecializationCreateDto>
{
    public SpecializationCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(128);
    }
}