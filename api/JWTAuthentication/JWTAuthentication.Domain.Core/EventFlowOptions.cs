using JWTAuthentication.Domain.Core.Aggregates;
using JWTAuthentication.Domain.Core.Commands;
using JWTAuthentication.Domain.Core.Configuration;
using JWTAuthentication.Domain.Core.Configuration.Cancellation;
using JWTAuthentication.Domain.Core.Configuration.Serialization;
using JWTAuthentication.Domain.Core.Core;
using JWTAuthentication.Domain.Core.Core.RetryStrategies;
using JWTAuthentication.Domain.Core.EventStores;
using JWTAuthentication.Domain.Core.EventStores.InMemory;
using JWTAuthentication.Domain.Core.Extensions;
using JWTAuthentication.Domain.Core.Jobs;
using JWTAuthentication.Domain.Core.Provided.Jobs;
using JWTAuthentication.Domain.Core.Queries;
using JWTAuthentication.Domain.Core.ReadStores;
using JWTAuthentication.Domain.Core.Sagas;
using JWTAuthentication.Domain.Core.Sagas.AggregateSagas;
using JWTAuthentication.Domain.Core.Snapshots;
using JWTAuthentication.Domain.Core.Snapshots.Stores;
using JWTAuthentication.Domain.Core.Snapshots.Stores.Null;
using JWTAuthentication.Domain.Core.Subscribers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Reflection;

namespace JWTAuthentication.Domain.Core
{
    public class EventFlowOptions : IEventFlowOptions
    {
        private readonly ConcurrentBag<Type> _aggregateEventTypes = new ConcurrentBag<Type>();
        private readonly ConcurrentBag<Type> _sagaTypes = new ConcurrentBag<Type>();
        private readonly ConcurrentBag<Type> _commandTypes = new ConcurrentBag<Type>();
        private readonly EventFlowConfiguration _eventFlowConfiguration = new EventFlowConfiguration();

        private readonly ConcurrentBag<Type> _jobTypes = new ConcurrentBag<Type>
            {
                typeof(PublishCommandJob),
                typeof(DispatchToAsynchronousEventSubscribersJob)
            };

        private readonly List<Type> _snapshotTypes = new List<Type>();

        public IServiceCollection ServiceCollection { get; }

        private EventFlowOptions(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;

            RegisterDefaults(ServiceCollection);
        }

        public static IEventFlowOptions New() =>
            new EventFlowOptions(
                new ServiceCollection()
                    .AddLogging(b => b
                        .SetMinimumLevel(LogLevel.Trace)
                        .AddConsole()));

        public static IEventFlowOptions New(IServiceCollection serviceCollection) => new EventFlowOptions(serviceCollection);

        public IEventFlowOptions ConfigureOptimisticConcurrencyRetry(int retries, TimeSpan delayBeforeRetry)
        {
            _eventFlowConfiguration.NumberOfRetriesOnOptimisticConcurrencyExceptions = retries;
            _eventFlowConfiguration.DelayBeforeRetryOnOptimisticConcurrencyExceptions = delayBeforeRetry;
            return this;
        }

        public IEventFlowOptions ConfigureThrowSubscriberExceptions(bool shouldThrow)
        {
            _eventFlowConfiguration.ThrowSubscriberExceptions = shouldThrow;
            return this;
        }

        public IEventFlowOptions Configure(Action<EventFlowConfiguration> configure)
        {
            configure(_eventFlowConfiguration);
            return this;
        }

        public IEventFlowOptions AddEvents(IEnumerable<Type> aggregateEventTypes)
        {
            foreach (var aggregateEventType in aggregateEventTypes)
            {
                if (!typeof(IAggregateEvent).GetTypeInfo().IsAssignableFrom(aggregateEventType))
                {
                    throw new ArgumentException($"Type {aggregateEventType.PrettyPrint()} is not a {typeof(IAggregateEvent).PrettyPrint()}");
                }
                _aggregateEventTypes.Add(aggregateEventType);
            }
            return this;
        }

        public IEventFlowOptions AddSagas(IEnumerable<Type> sagaTypes)
        {
            foreach (var sagaType in sagaTypes)
            {
                if (!typeof(ISaga).GetTypeInfo().IsAssignableFrom(sagaType))
                {
                    throw new ArgumentException($"Type {sagaType.PrettyPrint()} is not a {typeof(ISaga).PrettyPrint()}");
                }
                _sagaTypes.Add(sagaType);
            }
            return this;
        }

        public IEventFlowOptions AddCommands(IEnumerable<Type> commandTypes)
        {
            foreach (var commandType in commandTypes)
            {
                if (!typeof(ICommand).GetTypeInfo().IsAssignableFrom(commandType))
                {
                    throw new ArgumentException($"Type {commandType.PrettyPrint()} is not a {typeof(ICommand).PrettyPrint()}");
                }
                _commandTypes.Add(commandType);
            }
            return this;
        }

        public IEventFlowOptions AddJobs(IEnumerable<Type> jobTypes)
        {
            foreach (var jobType in jobTypes)
            {
                if (!typeof(IJob).GetTypeInfo().IsAssignableFrom(jobType))
                {
                    throw new ArgumentException($"Type {jobType.PrettyPrint()} is not a {typeof(IJob).PrettyPrint()}");
                }
                _jobTypes.Add(jobType);
            }
            return this;
        }

        public IEventFlowOptions AddSnapshots(IEnumerable<Type> snapshotTypes)
        {
            foreach (var snapshotType in snapshotTypes)
            {
                if (!typeof(ISnapshot).GetTypeInfo().IsAssignableFrom(snapshotType))
                {
                    throw new ArgumentException($"Type {snapshotType.PrettyPrint()} is not a {typeof(ISnapshot).PrettyPrint()}");
                }
                _snapshotTypes.Add(snapshotType);
            }
            return this;
        }

        private void RegisterDefaults(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMemoryCache();

            RegisterObsoleteDefaults(serviceCollection);

            // Default no-op resilience strategies
            serviceCollection.TryAddTransient<IAggregateStoreResilienceStrategy, NoAggregateStoreResilienceStrategy>();
            serviceCollection.TryAddTransient<IDispatchToReadStoresResilienceStrategy, NoDispatchToReadStoresResilienceStrategy>();
            serviceCollection.TryAddTransient<ISagaUpdateResilienceStrategy, NoSagaUpdateResilienceStrategy>();
            serviceCollection.TryAddTransient<IDispatchToSubscriberResilienceStrategy, NoDispatchToSubscriberResilienceStrategy>();

            serviceCollection.TryAddTransient<IDispatchToReadStores, DispatchToReadStores>();
            serviceCollection.TryAddTransient<IEventStore, EventStoreBase>();
            serviceCollection.TryAddSingleton<IEventUpgradeContextFactory, EventUpgradeContextFactory>();
            serviceCollection.TryAddSingleton<IEventPersistence, InMemoryEventPersistence>();
            serviceCollection.TryAddTransient<ICommandBus, CommandBus>();
            serviceCollection.TryAddTransient<IAggregateStore, AggregateStore>();
            serviceCollection.TryAddTransient<ISnapshotStore, SnapshotStore>();
            serviceCollection.TryAddTransient<ISnapshotSerializer, SnapshotSerializer>();
            serviceCollection.TryAddTransient<ISnapshotPersistence, NullSnapshotPersistence>();
            serviceCollection.TryAddTransient<ISnapshotUpgradeService, SnapshotUpgradeService>();
            serviceCollection.TryAddTransient<IReadModelPopulator, ReadModelPopulator>();
            serviceCollection.TryAddTransient<IEventJsonSerializer, EventJsonSerializer>();
            serviceCollection.TryAddTransient<IQueryProcessor, QueryProcessor>();
            serviceCollection.TryAddSingleton<IJsonSerializer, JsonSerializer>();
            serviceCollection.TryAddTransient<IJsonOptions, JsonOptions>();
            serviceCollection.TryAddTransient<IJobScheduler, InstantJobScheduler>();
            serviceCollection.TryAddTransient<IJobRunner, JobRunner>();
            serviceCollection.TryAddTransient<IOptimisticConcurrencyRetryStrategy, OptimisticConcurrencyRetryStrategy>();
            serviceCollection.TryAddSingleton<IEventUpgradeManager, EventUpgradeManager>();
            serviceCollection.TryAddTransient<IAggregateFactory, AggregateFactory>();
            serviceCollection.TryAddTransient<IReadModelDomainEventApplier, ReadModelDomainEventApplier>();
            serviceCollection.TryAddTransient<IDomainEventPublisher, DomainEventPublisher>();
            serviceCollection.TryAddTransient<ISerializedCommandPublisher, SerializedCommandPublisher>();
            serviceCollection.TryAddTransient<IDispatchToEventSubscribers, DispatchToEventSubscribers>();
            serviceCollection.TryAddSingleton<IDomainEventFactory, DomainEventFactory>();
            serviceCollection.TryAddTransient<ISagaStore, SagaAggregateStore>();
            serviceCollection.TryAddTransient<ISagaErrorHandler, SagaErrorHandler>();
            serviceCollection.TryAddTransient<IDispatchToSagas, DispatchToSagas>();
            serviceCollection.TryAddTransient(typeof(ISagaUpdater<,,,>), typeof(SagaUpdater<,,,>));
            serviceCollection.TryAddTransient<IEventFlowConfiguration>(_ => _eventFlowConfiguration);
            serviceCollection.TryAddTransient<ICancellationConfiguration>(_ => _eventFlowConfiguration);
            serviceCollection.TryAddTransient(typeof(ITransientFaultHandler<>), typeof(TransientFaultHandler<>));
            serviceCollection.TryAddSingleton(typeof(IReadModelFactory<>), typeof(ReadModelFactory<>));
            serviceCollection.TryAddSingleton<Func<Type, ISagaErrorHandler>>(_ => __ => null);

            // Definition services
            serviceCollection.TryAddSingleton<IEventDefinitionService, EventDefinitionService>();
            serviceCollection.TryAddSingleton<ISnapshotDefinitionService, SnapshotDefinitionService>();
            serviceCollection.TryAddSingleton<IJobDefinitionService, JobDefinitionService>();
            serviceCollection.TryAddSingleton<ISagaDefinitionService, SagaDefinitionService>();
            serviceCollection.TryAddSingleton<ICommandDefinitionService, CommandDefinitionService>();

            serviceCollection.TryAddSingleton<ILoadedVersionedTypes>(r => new LoadedVersionedTypes(
                _jobTypes,
                _commandTypes,
                _aggregateEventTypes,
                _sagaTypes,
                _snapshotTypes));
        }

        private void RegisterObsoleteDefaults(IServiceCollection serviceCollection)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            serviceCollection.TryAddTransient<ISnapshotSerilizer, SnapshotSerilizer>();
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
