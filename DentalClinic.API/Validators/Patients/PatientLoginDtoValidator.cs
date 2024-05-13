using DentalClinic.Shared.DTOs.Patients;

using FluentValidation;

namespace DentalClinic.API.Validators.Patients;

public class PatientLoginDtoValidator : AbstractValidator<PatientLoginDto>
{
    public PatientLoginDtoValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(128);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}