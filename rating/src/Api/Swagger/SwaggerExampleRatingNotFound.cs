using Core.ResponseContract;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Swagger;

public class SwaggerExampleRatingNotFound : IExamplesProvider<ErrorResponse>
{
    public ErrorResponse GetExamples()
    {
        return ErrorResponse.NotFound("EXAMPLEHANDLER");
    }
}