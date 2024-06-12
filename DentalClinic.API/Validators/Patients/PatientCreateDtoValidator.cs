using DentalClinic.Shared.DTOs.Patients;

using FluentValidation;

namespace DentalClinic.API.Validators.Patients;
public class PatientCreateDtoValidator : AbstractValidator<PatientCreateDto>
{
    public PatientCreateDtoValidator()
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
            .NotEmpty()
            .Must(BeLess18)
            .WithMessage("Patient must be less than 18 years old.");

        RuleFor(x => x.Address)
            .NotEmpty();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty();
    }

    private bool BeLess18(DateOnly birthDate)
    {
        if (birthDate >= DateOnly.FromDateTime(DateTime.UtcNow))
        {
            return false;
        }

        DateOnly currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

        if (currentDate.Year - birthDate.Year >= 18)
        {
            return false;
        }

        return true;
    }
}