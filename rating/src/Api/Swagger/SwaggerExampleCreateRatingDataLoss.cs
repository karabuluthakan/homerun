using Api.Command.Handler;
using Core.ResponseContract;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Swagger;

public class SwaggerExampleCreateRatingDataLoss : IExamplesProvider<ErrorResponse>
{
    public ErrorResponse GetExamples()
    {
        return ErrorResponse.DataLoss(nameof(CreateRatingCommandHandler), "RATING_RESOURCE_NOT_CREATED");
    }
}