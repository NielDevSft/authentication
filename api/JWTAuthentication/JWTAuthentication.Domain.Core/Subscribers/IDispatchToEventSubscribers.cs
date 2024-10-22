using JWTAuthentication.Domain.Core.Aggregates;

namespace JWTAuthentication.Domain.Core.Subscribers
{
    public interface IDispatchToEventSubscribers
    {
        Task DispatchToSynchronousSubscribersAsync(
            IReadOnlyCollection<IDomainEvent> domainEvents,
            CancellationToken cancellationToken);

        Task DispatchToAsynchronousSubscribersAsync(
            IDomainEvent domainEvent,
            CancellationToken cancellationToken);
    }
}