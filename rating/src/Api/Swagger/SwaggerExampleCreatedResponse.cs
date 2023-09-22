using Api.Command.Handler;
using Core.ResponseContract;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Swagger;

public class SwaggerExampleCreatedResponse : IExamplesProvider<CreatedResponse>
{
    public CreatedResponse GetExamples()
    {
        return CreatedResponse.Successful(Guid.NewGuid(), nameof(CreateRatingCommandHandler));
    }
}