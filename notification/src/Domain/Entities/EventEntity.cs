using Contract.Events.Abstract;
using Core.Domain.Entities;

namespace Domain.Entities;

public sealed class EventEntity : Entity
{
    public string EventType { get; set; }
    public int Version { get; set; }
    public int AggregateIdentifier { get; set; }
    public EventBase EventData { get; set; }
}