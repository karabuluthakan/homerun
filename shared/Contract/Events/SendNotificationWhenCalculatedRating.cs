using Contract.Events.Abstract;

namespace Contract.Events;

public class SendNotificationWhenCalculatedRating : EventBase
{
    public int CraftsmanId { get; set; }
    public int Count { get; set; }
    public double ScoreAverage { get; set; }

    public SendNotificationWhenCalculatedRating() : base(nameof(SendNotificationWhenCalculatedRating))
    {
    }
}