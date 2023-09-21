using Common.Swagger;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/ratings")]
[Route("api/v{version:apiVersion}/ratings")]
[Tags("Ratings")]
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
[SwaggerResponse(StatusCodes.Status400BadRequest, "VALIDATION_ERROR", typeof(SwaggerExampleValidationError))]
public class RatingsV1Controller : ControllerBase
{
    
}