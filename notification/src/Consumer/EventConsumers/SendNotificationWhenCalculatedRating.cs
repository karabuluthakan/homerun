using Contract.Events;
using Core.Extensions;
using Domain.Entities;
using Domain.Repository;
using MassTransit;

namespace Consumer.EventConsumers;

public sealed class SendNotificationWhenCalculatedRatingConsumer : IConsumer<SendNotificationWhenCalculatedRating>
{
    private readonly IEventEntityRepository _repository;
    private readonly ILogger<SendNotificationWhenCalculatedRatingConsumer> _logger;
    private const string Instance = nameof(SendNotificationWhenCalculatedRating);

    public SendNotificationWhenCalculatedRatingConsumer(
        IEventEntityRepository repository,
        ILogger<SendNotificationWhenCalculatedRatingConsumer> logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(logger);
        _repository = repository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendNotificationWhenCalculatedRating> context)
    {
        var message = context.Message;
        CancellationTokenSource cancellationTokenSource = new(TimeSpan.FromMilliseconds(20));
        var cancellationToken = cancellationTokenSource.Token;
        var eventType = message.GetType().Name;
        var existEvent = await _repository.GetAsync(x =>
            x.AggregateIdentifier.Equals(message.CraftsmanId) && x.EventType.Equals(eventType), cancellationToken);

        if (existEvent is not null)
        {
            var updateException = await _repository.UpdateAsync(existEvent, cancellationToken);
            if (updateException is not null)
            {
                _logger.LogCritical(updateException, "{instance}_EventNotUpdated with data {data}",
                    Instance, message.ToJson());
            }

            return;
        }

        EventEntity entity = new()
        {
            EventType = eventType,
            EventData = message,
            AggregateIdentifier = message.CraftsmanId,
            Version = 1
        };
        
        var exception = await _repository.AddAsync(entity, cancellationToken);
        if (exception is not null)
        {
            _logger.LogCritical(exception, "{instance}_EventNotCreated with data {data}",
                Instance, message.ToJson());
        }
    }
}