using Api.Command;
using FluentValidation;

namespace Api.ValidationRules;

public class UpdateRatingRequestValidation : AbstractValidator<UpdateRatingRequest>
{
    public UpdateRatingRequestValidation()
    {
        RuleFor(x => x.Id).NotNull().NotEqual(Guid.Empty);
        RuleFor(x => x.Score).NotNull();
        When(x => x.Score is not null, () =>
        {
            RuleFor(x => x.Score.Score)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(5);
        });
    }
}