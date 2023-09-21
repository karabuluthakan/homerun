using Api.Command;
using FluentValidation;

namespace Api.ValidationRules;

public class CreateRatingRequestValidation : AbstractValidator<CreateRatingRequest>
{
    public CreateRatingRequestValidation()
    {
        RuleFor(x => x.Dto).NotNull();
        When(x => x.Dto is not null, () => { RuleFor(x => x.Dto).SetValidator(new RatingDtoValidation()); });
    }
}