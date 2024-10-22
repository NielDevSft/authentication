using JWTAuthentication.Domain.Core.Core;

namespace JWTAuthentication.Domain.Core.Aggregates
{
    public interface IAggregateFactory
    {
        Task<TAggregate> CreateNewAggregateAsync<TAggregate, TIdentity>(TIdentity id)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity;
    }
}