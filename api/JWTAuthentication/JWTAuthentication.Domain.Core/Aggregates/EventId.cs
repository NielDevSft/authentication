using JWTAuthentication.Domain.Core.Core;

namespace JWTAuthentication.Domain.Core.Aggregates
{
    public class EventId : Identity<EventId>, IEventId
    {
        public EventId(string value) : base(value)
        {
        }
    }
}