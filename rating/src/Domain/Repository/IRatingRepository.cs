using Core.Repository;
using Domain.Entities;

namespace Domain.Repository;

public interface IRatingRepository : IRepository<RatingEntity, Guid>
{
    ValueTask<Exception?> UpdateScoreAsync(Guid id, int score);
}