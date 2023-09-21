namespace Core.Domain.Entities;

public interface IEntity
{
    public DateTimeOffset CreatedAt { get; }
}

public interface IEntity<out TKey> : IEntity
{
    public TKey Id { get; }
}