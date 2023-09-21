using Api.Query;
using FluentValidation;

namespace Api.ValidationRules;

public class GetRatingDetailRequestValidation : AbstractValidator<GetRatingDetailRequest>
{
    public GetRatingDetailRequestValidation()
    {
        RuleFor(x => x.Id).NotNull().NotEqual(Guid.Empty);
    }
}