using Domain.Entities;

namespace Domain.Builders;

public sealed class RatingEntityBuilder
{
    private readonly RatingEntity _entity;

    public RatingEntityBuilder()
    {
        _entity = new RatingEntity();
    }

    public static RatingEntityBuilder Init() => new();

    public RatingEntityBuilder CustomerId(int value)
    {
        _entity.CustomerId = value;
        return this;
    }

    public RatingEntityBuilder CraftsmanId(int value)
    {
        _entity.CraftsmanId = value;
        return this;
    }

    public RatingEntityBuilder TaskId(int value)
    {
        _entity.TaskId = value;
        return this;
    }

    public RatingEntityBuilder Score(int value)
    {
        if (value is < 1 or > 5) throw new ArgumentException();
        _entity.Score = value;
        return this;
    }

    public RatingEntity Build() => _entity;
}