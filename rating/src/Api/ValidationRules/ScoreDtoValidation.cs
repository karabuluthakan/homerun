using Domain.DataTransferObjects;
using FluentValidation;

namespace Api.ValidationRules;

public class ScoreDtoValidation : AbstractValidator<ScoreDto>
{
    public ScoreDtoValidation()
    {
        RuleFor(x => x.Score)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(5);
    }
}