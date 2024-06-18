using DentalClinic.Shared.DTOs.Dentists;

using FluentValidation;

namespace DentalClinic.API.Validators.Dentists;
public class DentistCreateDtoValidator : AbstractValidator<DentistCreateDto>
{
    public DentistCreateDtoValidator()
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

        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x => x.Password)
            .MinimumLength(8)
            .NotEmpty();

        RuleFor(x => x.BirthDate)
            .Must(BeMoreThan18LessThan60)
            .NotEmpty();
    }

    private bool BeMoreThan18LessThan60(DateOnly birthDate)
    {
        if (birthDate.ToDateTime(new TimeOnly(0, 0, 0)) >= DateTime.UtcNow)
        {
            return false;
        }

        DateOnly currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        int age = currentDate.Year - birthDate.Year;

        if (age >= 18 && age < 60)
        {
            return true;
        }

        return false;
    }
}