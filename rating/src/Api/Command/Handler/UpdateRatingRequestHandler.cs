using Core.ResponseContract;
using Core.ResponseContract.Abstract;
using Domain.Repository;
using MediatR;

namespace Api.Command.Handler;

public sealed class UpdateRatingRequestHandler : IRequestHandler<UpdateRatingRequest, IResponse>
{
    private const string Instance = nameof(UpdateRatingRequestHandler);
    private readonly IRatingRepository _repository;
    private readonly ILogger<UpdateRatingRequestHandler> _logger;
    private readonly IMediator _mediator;

    public UpdateRatingRequestHandler(
        IRatingRepository repository,
        ILogger<UpdateRatingRequestHandler> logger,
        IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(mediator);
        _repository = repository;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IResponse> Handle(UpdateRatingRequest request, CancellationToken cancellationToken)
    {
        var id = request.Id;
        var entity = await _repository.GetAsync(x => x.Id.Equals(id), cancellationToken);
        if (entity is null) return ErrorResponse.NotFound(Instance);

        var exception = await _repository.UpdateScoreAsync(id, request.Score.Score);
        if (exception is not null)
        {
            const string detail = "RATING_RESOURCE_NOT_UPDATED";
            _logger.LogCritical(exception, detail);
            return ErrorResponse.DataLoss(Instance, detail);
        }

        await _mediator.Publish(new CalculatingRatingNotification(entity), cancellationToken);
        return NoContentResponse.Successful(Instance);
    }
}