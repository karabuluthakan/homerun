using Domain.DataTransferObjects;
using FluentValidation;

namespace Api.ValidationRules;

public class RatingDtoValidation : AbstractValidator<RatingDto>
{
    public RatingDtoValidation()
    {
        RuleFor(x => x.Score)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(5);

        RuleFor(x => x.CraftsmanId)
            .NotNull()
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.CustomerId)
            .NotNull()
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.TaskId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}