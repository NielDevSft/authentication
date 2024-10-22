using JWTAuthentication.Domain.Core.Core;
using JWTAuthentication.Domain.Core.EventStores;
using JWTAuthentication.Domain.Core.Snapshots;

namespace JWTAuthentication.Domain.Core.Aggregates
{
    public interface IAggregateRoot
    {
        IAggregateName Name { get; }
        int Version { get; }
        IEnumerable<IUncommittedEvent> UncommittedEvents { get; }
        IEnumerable<ISourceId> PreviousSourceIds { get; }
        bool IsNew { get; }

        Task<IReadOnlyCollection<IDomainEvent>> CommitAsync(IEventStore eventStore, ISnapshotStore snapshotStore, ISourceId sourceId, CancellationToken cancellationToken);

        bool HasSourceId(ISourceId sourceId);

        void ApplyEvents(IReadOnlyCollection<IDomainEvent> domainEvents);

        IIdentity GetIdentity();

        Task LoadAsync(IEventStore eventStore, ISnapshotStore snapshotStore, CancellationToken cancellationToken);
    }

    public interface IAggregateRoot<out TIdentity> : IAggregateRoot
        where TIdentity : IIdentity
    {
        TIdentity Id { get; }
    }
}