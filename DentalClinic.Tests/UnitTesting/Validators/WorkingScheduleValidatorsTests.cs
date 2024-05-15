using DentalClinic.API.Validators.WorkingSchedules;
using DentalClinic.Shared.DTOs.WorkingSchedules;

using FluentValidation.TestHelper;

namespace DentalClinic.Tests.UnitTesting.Validators;
public class WorkingScheduleValidatorsTests
{
    [Fact]
    public void WorkingScheduleCreateDto_WhenPropertiesInvalid_ShouldThrowException()
    {
        //arrange
        WorkingScheduleCreateDto wsCreateDto = new()
        {
            Start = new TimeOnly(8, 0, 0),
            End = new TimeOnly(7, 0, 0),
            WorkingDay = "Понедельник"
        };

        WorkingScheduleCreateDtoValidator validator = new();
        //act
        var validationResult = validator.TestValidate(wsCreateDto);
        //assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Start);
        validationResult.ShouldHaveValidationErrorFor(x => x.End);
    }

    [Fact]
    public void WorkingScheduleCreateDto_WhenPropertiesValid_ShouldPass()
    {
        //arrange
        WorkingScheduleCreateDto wsCreateDto = new()
        {
            Start = new TimeOnly(8, 0, 0),
            End = new TimeOnly(14, 0, 0),
            WorkingDay = "Понедельник"
        };

        WorkingScheduleCreateDtoValidator validator = new();
        //act
        var validationResult = validator.TestValidate(wsCreateDto);
        //assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void WorkingScheduleUpdateDto_WhenPropertiesInvalid_ShouldThrowException()
    {
        //arrange
        WorkingScheduleUpdateDto wsUpdateDto = new()
        {
            Start = new TimeOnly(8, 0, 0),
            End = new TimeOnly(7, 0, 0),
            WorkingDay = "Понедельник"
        };

        WorkingScheduleUpdateDtoValidator validator = new();
        //act
        var validationResult = validator.TestValidate(wsUpdateDto);
        //assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Start);
        validationResult.ShouldHaveValidationErrorFor(x => x.End);
    }

    [Fact]
    public void WorkingScheduleUpdateDto_WhenPropertiesValid_ShouldPass()
    {
        //arrange
        WorkingScheduleUpdateDto wsUpdateDto = new()
        {
            Start = new TimeOnly(8, 0, 0),
            End = new TimeOnly(14, 0, 0),
            WorkingDay = "Понедельник"
        };

        WorkingScheduleUpdateDtoValidator validator = new();
        //act
        var validationResult = validator.TestValidate(wsUpdateDto);
        //assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }
}
