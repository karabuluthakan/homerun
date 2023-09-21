using Core.Repository;
using Domain.Entities;

namespace Domain.Repository;

public interface IEventEntityRepository : IRepository<EventEntity,string>
{
}