using JWTAuthentication.Domain.Core.Core;

namespace JWTAuthentication.Domain.Core.Aggregates
{
    public interface IEventId : ISourceId
    {
        Guid GetGuid();
    }
}
