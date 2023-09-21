namespace Core.Domain.Entities;

public interface IEntity
{
    public string Id { get; }
    public DateTimeOffset CreatedAt { get; }
}