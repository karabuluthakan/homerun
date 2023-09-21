using Core.Repository;
using Domain.Entities;

namespace Domain.Repository;

public interface IRatingRepository : IRepository<RatingEntity,Guid>
{
}