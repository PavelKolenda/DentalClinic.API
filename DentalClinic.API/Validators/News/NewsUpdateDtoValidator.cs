using DentalClinic.Shared.DTOs.News;

using FluentValidation;

namespace DentalClinic.API.Validators.News;

public class NewsUpdateDtoValidator : AbstractValidator<NewsUpdateDto>
{
    public NewsUpdateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(10000);

        RuleFor(x => x.CreatedAt)
            .NotEmpty();
    }
}

