namespace Core.Domain.Entities;

public abstract class Entity : IEntity
{
    public string Id { get; set; }
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;
}