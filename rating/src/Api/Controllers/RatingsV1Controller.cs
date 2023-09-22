using Api.Command;
using Api.Extensions;
using Api.Query;
using Api.Swagger;
using Core.ResponseContract;
using Domain.DataTransferObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/ratings")]
[Tags("Ratings [Version = 1.0]")]
public class RatingsV1Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public RatingsV1Controller(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RatingDto>))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SwaggerExamplesGetAllRating))]
    public async ValueTask<IActionResult> Index()
    {
        CancellationTokenSource cancellationTokenSource = new();
        var cancellationToken = cancellationTokenSource.Token;
        var request = new GetAllRatingDetailRequest();
        var response = await _mediator.Send(request, cancellationToken);
        return this.ToResponse(response);
    }

    [HttpGet("{ratingId:guid}")]
    [
        ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RatingDto)),
        ProducesResponseType(StatusCodes.Status400BadRequest),
        ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(SwaggerExampleRatingNotFound))
    ]
    [
        SwaggerResponse(StatusCodes.Status200OK, Type = typeof(RatingDto)),
        SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(SwaggerExampleRatingNotFound))
    ]
    public async ValueTask<IActionResult> Show([FromRoute] Guid ratingId)
    {
        CancellationTokenSource cancellationTokenSource = new();
        var cancellationToken = cancellationTokenSource.Token;
        var request = new GetRatingDetailRequest { Id = ratingId };
        var response = await _mediator.Send(request, cancellationToken);
        return this.ToResponse(response);
    }

    [HttpPost]
    [
        ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatedResponse)),
        ProducesResponseType(StatusCodes.Status400BadRequest),
        ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError,
            Type = typeof(SwaggerExampleCreateRatingDataLoss))
    ]
    [
        SwaggerResponse(StatusCodes.Status201Created, Type = typeof(CreatedResponse)),
        SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(SwaggerExampleCreateRatingDataLoss))
    ]
    public async ValueTask<IActionResult> Create([FromBody] RatingDto body)
    {
        CancellationTokenSource cancellationTokenSource = new();
        var cancellationToken = cancellationTokenSource.Token;
        var request = new CreateRatingRequest { Dto = body };
        var response = await _mediator.Send(request, cancellationToken);
        return this.ToResponse(response);
    }

    [HttpPatch("{ratingId:guid}")]
    [
        ProducesResponseType(StatusCodes.Status204NoContent),
        ProducesResponseType(StatusCodes.Status400BadRequest),
        ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(SwaggerExampleRatingNotFound))
    ]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(SwaggerExampleRatingNotFound)),]
    public async ValueTask<IActionResult> UpdateScore([FromRoute] Guid ratingId, [FromBody] ScoreDto body)
    {
        CancellationTokenSource cancellationTokenSource = new();
        var cancellationToken = cancellationTokenSource.Token;
        var request = new UpdateRatingRequest { Id = ratingId, Score = body };
        var response = await _mediator.Send(request, cancellationToken);
        return this.ToResponse(response);
    }
}