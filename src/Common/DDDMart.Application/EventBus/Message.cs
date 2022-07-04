
namespace DDDMart.Application.EventBus
{
    public sealed class Message
    {
        public Message(string body, string eventType, DateTime timestamp)
        {
            Body = body;
            EventType = eventType;
            Timestamp = timestamp;
        }

        public readonly string Body;
        public readonly string EventType;
        public readonly DateTime Timestamp;
    }
}
