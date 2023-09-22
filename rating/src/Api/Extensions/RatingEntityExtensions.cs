using Domain.DataTransferObjects;
using Domain.Entities;

namespace Api.Extensions;

public static class RatingEntityExtensions
{
    public static RatingDto ToDto(this RatingEntity entity)
    {
        return new RatingDto
        {
            CustomerId = entity.CustomerId,
            CraftsmanId = entity.CraftsmanId,
            TaskId = entity.TaskId,
            Score = entity.Score
        };
    }
}