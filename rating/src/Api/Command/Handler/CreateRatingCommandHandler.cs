using Core.ResponseContract;
using Core.ResponseContract.Abstract;
using Domain.Builders;
using Domain.Repository;
using MediatR;

namespace Api.Command.Handler;

public sealed class CreateRatingCommandHandler : IRequestHandler<CreateRatingRequest, IResponse>
{
    private const string Instance = nameof(CreateRatingCommandHandler);
    private readonly IRatingRepository _repository;
    private readonly ILogger<CreateRatingCommandHandler> _logger;
    private readonly IMediator _mediator;

    public CreateRatingCommandHandler(
        IRatingRepository repository,
        ILogger<CreateRatingCommandHandler> logger,
        IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(mediator);
        _repository = repository;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IResponse> Handle(CreateRatingRequest request, CancellationToken cancellationToken)
    {
        var entity = RatingEntityBuilder
            .Init()
            .CraftsmanId(request.Dto.CraftsmanId)
            .TaskId(request.Dto.TaskId)
            .CustomerId(request.Dto.CustomerId)
            .Score(request.Dto.Score)
            .Build();

        var exception = await _repository.AddAsync(entity, cancellationToken);
        if (exception is not null)
        {
            const string detail = "RATING_RESOURCE_NOT_CREATED";
            _logger.LogCritical(exception, detail);
            return ErrorResponse.DataLoss(Instance, detail);
        }

        await _mediator.Publish(new CalculatingRatingNotification(entity), cancellationToken);
        return CreatedResponse.Successful(entity.Id, Instance);
    }
}