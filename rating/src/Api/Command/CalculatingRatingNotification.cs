using Domain.Entities;
using MediatR;

namespace Api.Command;

public sealed class CalculatingRatingNotification : INotification
{
    public RatingEntity Entity { get; }

    public CalculatingRatingNotification(RatingEntity entity)
    {
        Entity = entity;
    }
}