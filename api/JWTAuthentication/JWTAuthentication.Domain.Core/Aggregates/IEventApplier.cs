using JWTAuthentication.Domain.Core.Core;

namespace JWTAuthentication.Domain.Core.Aggregates
{
    public interface IEventApplier<TAggregate, TIdentity>
        where TAggregate : IAggregateRoot<TIdentity>
        where TIdentity : IIdentity
    {
        bool Apply(TAggregate aggregate, IAggregateEvent<TAggregate, TIdentity> aggregateEvent);
    }
}