using DentalClinic.API.Validators.Patients;
using DentalClinic.Shared.DTOs.Patients;

using FluentValidation.TestHelper;

namespace DentalClinic.Tests.UnitTesting.Validators;
public class PatientValidatorsTests
{
    [Fact]
    public void PatientCreateDto_WhenPropertiesInvalid_ShouldThrowException()
    {
        PatientCreateDto patient = new()
        {
            Name = "",
            Surname = "",
            Patronymic = "",
            BirthDate = new DateOnly(1999, 1, 1),
            Email = "",
            Password = "",
            Address = "",
            PhoneNumber = ""
        };

        PatientCreateDtoValidator validator = new();
        var validationResult = validator.TestValidate(patient);

        validationResult.ShouldHaveValidationErrorFor(x => x.Name);
        validationResult.ShouldHaveValidationErrorFor(x => x.Surname);
        validationResult.ShouldHaveValidationErrorFor(x => x.Email);
        validationResult.ShouldHaveValidationErrorFor(x => x.Password);
        validationResult.ShouldHaveValidationErrorFor(x => x.Address);
        validationResult.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Fact]
    public void PatientCreateDto_WhenPropertiesValid_ShouldPass()
    {
        PatientCreateDto patient = new()
        {
            Name = "Тест",
            Surname = "Тест",
            Patronymic = "Тест",
            BirthDate = new DateOnly(2012, 12, 12),
            Email = "test@gmail.com",
            Password = "testtesttest",
            PhoneNumber = "+375296589765",
            Address = "Соломовая 6a"
        };

        PatientCreateDtoValidator validator = new();
        var validationResult = validator.TestValidate(patient);
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void PatientLoginDto_WhenPropertiesInvalid_ShouldThrowException()
    {
        PatientLoginDto patient = new()
        {
            Email = "f@",
            Password = "12345"
        };

        PatientLoginDtoValidator validator = new();
        var validationResult = validator.TestValidate(patient);

        validationResult.ShouldHaveValidationErrorFor(x => x.Email);
        validationResult.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void PatientLoginDto_WhenPropertiesValid_ShouldPass()
    {
        PatientLoginDto patient = new()
        {
            Email = "realemail@gmail.com",
            Password = "strongPassword"
        };

        PatientLoginDtoValidator validator = new();
        var validationResult = validator.TestValidate(patient);
        validationResult.ShouldNotHaveAnyValidationErrors();
    }
}