using JWTAuthentication.Domain.Core.Core;
using JWTAuthentication.Domain.Core.Extensions;

namespace JWTAuthentication.Domain.Core.Aggregates
{
    public abstract class AggregateEvent<TAggregate, TIdentity> : IAggregateEvent<TAggregate, TIdentity>
        where TAggregate : IAggregateRoot<TIdentity>
        where TIdentity : IIdentity
    {
        public override string ToString()
        {
            return $"{typeof(TAggregate).PrettyPrint()}/{GetType().PrettyPrint()}";
        }
    }
}