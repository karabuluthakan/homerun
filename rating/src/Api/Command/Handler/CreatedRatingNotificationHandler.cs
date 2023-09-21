using Contract;
using Contract.Events;
using Core.Extensions;
using Domain.CrossCuttingConcern.Caching;
using Domain.DataTransferObjects;
using MassTransit;
using MediatR;

namespace Api.Command.Handler;

public sealed class CreatedRatingNotificationHandler : INotificationHandler<CreatedRatingNotification>
{
    private readonly ICacheDispatcher _cache;
    private readonly IBus _bus;
    private readonly ILogger<CreatedRatingNotificationHandler> _logger;

    public CreatedRatingNotificationHandler(
        ICacheDispatcher cache,
        IBus bus,
        ILogger<CreatedRatingNotificationHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(bus);
        ArgumentNullException.ThrowIfNull(logger);
        _cache = cache;
        _bus = bus;
        _logger = logger;
    }

    public async Task Handle(CreatedRatingNotification notification, CancellationToken cancellationToken)
    {
        var entity = notification.Entity;
        var rating = await GetRatingsAsync(entity.CraftsmanId, cancellationToken);
        rating.Count += 1;
        rating.ScoreAverage += entity.Score / rating.Count;

        var ratings = new CalculatedRating
            { Ratings = new Dictionary<int, CalculatedRatingItem> { { entity.CraftsmanId, rating } } };

        var isCached = _cache.Set(CacheKeys.CalculatedRatingKey, ratings, TimeSpan.FromHours(1));
        if (!isCached)
        {
            _logger.LogCritical(message: "CreatedRatingNotification_is_not_cached with data : {data}", entity.ToJson());
            return;
        }

        var sendEndpoint = await _bus.GetSendEndpoint(
            new Uri($"queue:{QueueNames.SendNotificationWhenCalculatedRatingQueueName}"));

        SendNotificationWhenCalculatedRating message = new()
        {
            CraftsmanId = entity.CraftsmanId,
            ScoreAverage = rating.ScoreAverage,
            Count = rating.Count
        };

        await sendEndpoint.Send(message, cancellationToken);
    }

    private async ValueTask<CalculatedRatingItem> GetRatingsAsync(int key, CancellationToken cancellationToken)
    {
        var cachedData = await _cache.GetAsync<CalculatedRating>(CacheKeys.CalculatedRatingKey, cancellationToken);
        var ratings = cachedData?.Ratings ?? new Dictionary<int, CalculatedRatingItem>();
        return ratings.TryGetValue(key, out var item) ? item : new CalculatedRatingItem();
    }
}