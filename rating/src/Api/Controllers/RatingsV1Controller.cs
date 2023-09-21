using Api.Command;
using Api.Extensions;
using Common.Swagger;
using Domain.DataTransferObjects;
using MediatR;
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
    private readonly IMediator _mediator;

    public RatingsV1Controller(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    [HttpGet]
    public async ValueTask<IActionResult> Get()
    {
        return Ok();
    }

    [HttpPost]
    public async ValueTask<IActionResult> Create(
        [FromBody] RatingDto body,
        CancellationToken cancellationToken = default)
    {
        var request = new CreateRatingRequest { Dto = body };
        var response = await _mediator.Send(request, cancellationToken);
        return this.ToResponse(response);
    }

    [HttpPatch("{ratingId:guid}")]
    public async ValueTask<IActionResult> UpdateScore(
        [FromRoute] Guid ratingId,
        [FromBody] ScoreDto body,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateRatingRequest { Id = ratingId, Score = body };
        var response = await _mediator.Send(request, cancellationToken);
        return this.ToResponse(response);
    }
}