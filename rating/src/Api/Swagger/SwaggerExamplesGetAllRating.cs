using Domain.DataTransferObjects;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Swagger;

public class SwaggerExamplesGetAllRating : IMultipleExamplesProvider<List<RatingDto>>
{
    public IEnumerable<SwaggerExample<List<RatingDto>>> GetExamples()
    {
        yield return new SwaggerExample<List<RatingDto>>
        {
            Name = "Empty",
            Value = new List<RatingDto>()
        };
        yield return new SwaggerExample<List<RatingDto>>
        {
            Name = "Full",
            Value = new List<RatingDto>
            {
                new()
                {
                    CustomerId = 2,
                    CraftsmanId = 2,
                    TaskId = 2,
                    Score = 4
                },
                new()
                {
                    CustomerId = 3,
                    CraftsmanId = 3,
                    TaskId = 3,
                    Score = 5
                }
            }
        };
    }
}