using Domain.DataTransferObjects;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Swagger;

public class SwaggerExampleRatingDto : IExamplesProvider<RatingDto>
{
    public RatingDto GetExamples()
    {
        return new RatingDto
        {
            CustomerId = 2,
            CraftsmanId = 2,
            TaskId = 2,
            Score = 4
        };
    }
}