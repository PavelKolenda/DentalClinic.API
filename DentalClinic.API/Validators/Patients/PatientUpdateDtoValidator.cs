using DentalClinic.Shared.DTOs.Patients;

using FluentValidation;

namespace DentalClinic.API.Validators.Patients;

public class PatientUpdateDtoValidator : AbstractValidator<PatientUpdateDto>
{
    public PatientUpdateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MinimumLength(2);

        RuleFor(x => x.Surname)
            .NotEmpty()
            .MaximumLength(75)
            .MinimumLength(2);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(128);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.BirthDate)
            .NotEmpty();
    }
}