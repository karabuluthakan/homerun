using Domain.Entities;
using MediatR;

namespace Api.Command;

public sealed class CreatedRatingNotification : INotification
{
    public RatingEntity Entity { get; }

    public CreatedRatingNotification(RatingEntity entity)
    {
        Entity = entity;
    }
}