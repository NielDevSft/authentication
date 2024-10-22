using JWTAuthentication.Domain.Core.Aggregates;
using JWTAuthentication.Domain.Core.Configuration;
using JWTAuthentication.Domain.Core.Configuration.Cancellation;
using JWTAuthentication.Domain.Core.Core;
using JWTAuthentication.Domain.Core.Jobs;
using JWTAuthentication.Domain.Core.Provided.Jobs;
using JWTAuthentication.Domain.Core.ReadStores;
using JWTAuthentication.Domain.Core.Sagas;

namespace JWTAuthentication.Domain.Core.Subscribers
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        private readonly IDispatchToEventSubscribers _dispatchToEventSubscribers;
        private readonly IDispatchToSagas _dispatchToSagas;
        private readonly IJobScheduler _jobScheduler;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventFlowConfiguration _eventFlowConfiguration;
        private readonly ICancellationConfiguration _cancellationConfiguration;
        private readonly IDispatchToReadStores _dispatchToReadStores;
        private readonly IReadOnlyCollection<ISubscribeSynchronousToAll> _subscribeSynchronousToAlls;

        public DomainEventPublisher(
            IDispatchToEventSubscribers dispatchToEventSubscribers,
            IDispatchToSagas dispatchToSagas,
            IJobScheduler jobScheduler,
            IServiceProvider serviceProvider,
            IEventFlowConfiguration eventFlowConfiguration,
            IEnumerable<ISubscribeSynchronousToAll> subscribeSynchronousToAlls,
            ICancellationConfiguration cancellationConfiguration,
            IDispatchToReadStores dispatchToReadStores)
        {
            _dispatchToEventSubscribers = dispatchToEventSubscribers;
            _dispatchToSagas = dispatchToSagas;
            _jobScheduler = jobScheduler;
            _serviceProvider = serviceProvider;
            _eventFlowConfiguration = eventFlowConfiguration;
            _cancellationConfiguration = cancellationConfiguration;
            _dispatchToReadStores = dispatchToReadStores;
            _subscribeSynchronousToAlls = subscribeSynchronousToAlls.ToList();
        }

        public Task PublishAsync<TAggregate, TIdentity>(
            TIdentity id,
            IReadOnlyCollection<IDomainEvent> domainEvents,
            CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity
        {
            return PublishAsync(
                domainEvents,
                cancellationToken);
        }

        public async Task PublishAsync(
            IReadOnlyCollection<IDomainEvent> domainEvents,
            CancellationToken cancellationToken)
        {
            cancellationToken = _cancellationConfiguration.Limit(cancellationToken, CancellationBoundary.BeforeUpdatingReadStores);
            await PublishToReadStoresAsync(domainEvents, cancellationToken).ConfigureAwait(false);

            cancellationToken = _cancellationConfiguration.Limit(cancellationToken, CancellationBoundary.BeforeNotifyingSubscribers);
            await PublishToSubscribersOfAllEventsAsync(domainEvents, cancellationToken).ConfigureAwait(false);

            // Update subscriptions AFTER read stores have been updated
            await PublishToSynchronousSubscribersAsync(domainEvents, cancellationToken).ConfigureAwait(false);
            await PublishToAsynchronousSubscribersAsync(domainEvents, cancellationToken).ConfigureAwait(false);

            await PublishToSagasAsync(domainEvents, cancellationToken).ConfigureAwait(false);
        }

        private async Task PublishToReadStoresAsync(
            IReadOnlyCollection<IDomainEvent> domainEvents,
            CancellationToken cancellationToken)
        {
            await _dispatchToReadStores.DispatchAsync(
                domainEvents,
                cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task PublishToSubscribersOfAllEventsAsync(
            IReadOnlyCollection<IDomainEvent> domainEvents,
            CancellationToken cancellationToken)
        {
            var handle = _subscribeSynchronousToAlls
                .Select(s => s.HandleAsync(domainEvents, cancellationToken));
            await Task.WhenAll(handle).ConfigureAwait(false);
        }

        private async Task PublishToSynchronousSubscribersAsync(
            IReadOnlyCollection<IDomainEvent> domainEvents,
            CancellationToken cancellationToken)
        {
            await _dispatchToEventSubscribers.DispatchToSynchronousSubscribersAsync(domainEvents, cancellationToken).ConfigureAwait(false);
        }

        private async Task PublishToAsynchronousSubscribersAsync(
            IEnumerable<IDomainEvent> domainEvents,
            CancellationToken cancellationToken)
        {
            if (_eventFlowConfiguration.IsAsynchronousSubscribersEnabled)
            {
                await Task.WhenAll(domainEvents.Select(
                        d => _jobScheduler.ScheduleNowAsync(
                            DispatchToAsynchronousEventSubscribersJob.Create(d, _serviceProvider), cancellationToken)))
                    .ConfigureAwait(false);
            }
        }

        private async Task PublishToSagasAsync(
            IReadOnlyCollection<IDomainEvent> domainEvents,
            CancellationToken cancellationToken)
        {
            await _dispatchToSagas.ProcessAsync(domainEvents, cancellationToken).ConfigureAwait(false);
        }
    }
}
