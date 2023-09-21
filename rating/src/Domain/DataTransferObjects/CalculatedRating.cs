using Core.Domain.DataTransferObjects;

namespace Domain.DataTransferObjects;

public class CalculatedRating : IDto
{
    public Dictionary<int, CalculatedRatingItem> Ratings { get; set; } = new();
}

public class CalculatedRatingItem
{
    public int Count { get; set; }
    public double ScoreAverage { get; set; }
}