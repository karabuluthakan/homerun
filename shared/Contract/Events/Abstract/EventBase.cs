namespace Contract.Events.Abstract;

public abstract class EventBase
{
    public int Version { get; set; }
    public string Type { get; set; }

    protected EventBase(string type)
    {
        Type = type;
    }
}