using JWTAuthentication.Domain.Core.Core;
using JWTAuthentication.Domain.Core.Core.VersionedTypes;

namespace JWTAuthentication.Domain.Core.Aggregates
{
    public interface IAggregateEvent : IVersionedType
    {
    }

    public interface IAggregateEvent<TAggregate, TIdentity> : IAggregateEvent
        where TAggregate : IAggregateRoot<TIdentity>
        where TIdentity : IIdentity
    {
    }
}