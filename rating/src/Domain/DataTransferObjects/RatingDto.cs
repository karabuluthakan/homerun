using Core.Domain.DataTransferObjects;

namespace Domain.DataTransferObjects;

public sealed class RatingDto : IDto
{
    public int CustomerId { get; set; }
    public int CraftsmanId { get; set; }
    public int TaskId { get; set; }
    public int Score { get; set; }
}