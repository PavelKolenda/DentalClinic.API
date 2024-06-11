using DentalClinic.API.Validators.Dentists;
using DentalClinic.Shared.DTOs.Dentists;

using FluentValidation.TestHelper;

namespace DentalClinic.Tests.UnitTesting.Validators;

public class DentistValidatorsTests
{
    public DentistValidatorsTests()
    {
    }

    [Fact]
    public void DentistCreateDto_WhenPropertiesInvalid_ShouldThrowException()
    {
        DentistCreateDto dentist = new()
        {
            Name = "",
            Surname = "",
            Patronymic = "",
            CabinetNumber = 0,
            Specialization = "",
            Email = "",
            Password = "",
            BirthDate = new DateOnly(1900, 1, 1)
        };

        DentistCreateDtoValidator validator = new();
        var validationResult = validator.TestValidate(dentist);

        validationResult.ShouldHaveValidationErrorFor(x => x.Name);
        validationResult.ShouldHaveValidationErrorFor(x => x.Surname);
        validationResult.ShouldHaveValidationErrorFor(x => x.CabinetNumber);
        validationResult.ShouldHaveValidationErrorFor(x => x.Specialization);
        validationResult.ShouldHaveValidationErrorFor(x => x.Email);
        validationResult.ShouldHaveValidationErrorFor(x => x.Password);
        validationResult.ShouldHaveValidationErrorFor(x => x.BirthDate);
    }

    [Fact]
    public void DentistCreateDto_WhenPropertiesValid_ShouldPass()
    {
        DentistCreateDto dentist = new()
        {
            Name = "test",
            Surname = "test",
            Patronymic = "test",
            CabinetNumber = 10,
            Specialization = "test",
            Email = "test@gmail.com",
            Password = "testgmailcom",
            BirthDate = new DateOnly(1999, 1, 1)
        };

        DentistCreateDtoValidator validator = new();
        var validationResult = validator.TestValidate(dentist);

        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void DentistUpdateDto_WhenPropertiesInvalid_ShouldThrowExceptions()
    {
        DentistUpdateDto dentist = new()
        {
            Name = "",
            Surname = "",
            Patronymic = "",
            CabinetNumber = -12,
            Specialization = ""
        };

        DentistUpdateDtoValidator validator = new();
        var validationResult = validator.TestValidate(dentist);

        validationResult.ShouldHaveValidationErrorFor(x => x.Name);
        validationResult.ShouldHaveValidationErrorFor(x => x.Surname);
        validationResult.ShouldHaveValidationErrorFor(x => x.CabinetNumber);
        validationResult.ShouldHaveValidationErrorFor(x => x.Specialization);
    }

    [Fact]
    public void DentistUpdateDto_WhenPropertiesValid_ShouldPass()
    {
        DentistUpdateDto dentist = new()
        {
            Name = "test",
            Surname = "test",
            Patronymic = "test",
            CabinetNumber = 10,
            Specialization = "test"
        };

        DentistUpdateDtoValidator validator = new();
        var validationResult = validator.TestValidate(dentist);

        validationResult.ShouldNotHaveAnyValidationErrors();
    }
}