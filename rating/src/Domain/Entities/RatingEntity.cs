using Core.Domain.Entities;

namespace Domain.Entities;

[Serializable]
public sealed class RatingEntity : Entity<Guid>
{
    public int CustomerId { get; set; }
    public int CraftsmanId { get; set; }
    public int TaskId { get; set; }
    public int Score { get; set; }

    public RatingEntity()
    {
        Id = Guid.NewGuid();
    }
}