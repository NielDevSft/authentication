using JWTAuthentication.Domain.Core.Configuration.Cancellation;

namespace JWTAuthentication.Domain.Core.Configuration
{
    public class EventFlowConfiguration : IEventFlowConfiguration, ICancellationConfiguration
    {
        /// <summary>
        /// Number of events to load from the event persistence when read models
        /// are populated.
        /// </summary>
        /// <remarks>Defaults to 200</remarks>
        public int LoadReadModelEventPageSize { get; set; }

        /// <summary>
        /// Number of events to batch together when updating read models
        /// </summary>
        /// <remarks>Defaults to 200</remarks>
        public int PopulateReadModelEventPageSize { get; set; }

        /// <summary>
        /// Use by <c>OptimisticConcurrencyRetryStrategy</c> to determine the number
        /// of retries when an optimistic concurrency exceptions is thrown from the
        /// event persistence.
        /// 
        /// If more fine grained control of is needed, a custom implementation of
        /// <c>IOptimisticConcurrencyRetryStrategy</c> should be provided.
        /// </summary>
        /// <remarks>Defaults to 4</remarks>
        public int NumberOfRetriesOnOptimisticConcurrencyExceptions { get; set; }

        /// <summary>
        /// Use by <c>OptimisticConcurrencyRetryStrategy</c> to determine the delay
        /// between retries when an optimistic concurrency exceptions is thrown from the
        /// event persistence.
        /// 
        /// If more fine grained control of is needed, a custom implementation of
        /// <c>IOptimisticConcurrencyRetryStrategy</c> should be provided.
        /// </summary>
        /// <remarks>Defaults to 100 ms</remarks>
        public TimeSpan DelayBeforeRetryOnOptimisticConcurrencyExceptions { get; set; }

        /// <summary>
        /// Should EventFlow throw exceptions thrown by subscribers when publishing
        /// domain events.
        /// </summary>
        /// <remarks>Defaults to false</remarks>
        public bool ThrowSubscriberExceptions { get; set; }

        /// <summary>
        /// Should EventFlow schedule a job to invoke asynchronous domain event
        /// subscribers
        /// </summary>
        /// <remarks>Defaults to false</remarks>
        public bool IsAsynchronousSubscribersEnabled { get; set; }

        /// <summary>
        /// The point of no return in the processing chain. Before
        /// this point, cancellation is possible. After this point, the passed
        /// cancellation token is ignored.
        /// </summary>
        /// <remarks>Defaults to
        /// <see cref="CancellationBoundary.BeforeCommittingEvents"/></remarks>
        public CancellationBoundary CancellationBoundary { get; set; }

        public bool ForwardOptimisticConcurrencyExceptions { get; set; }

        internal EventFlowConfiguration()
        {
            LoadReadModelEventPageSize = 200;
            PopulateReadModelEventPageSize = 10000;
            NumberOfRetriesOnOptimisticConcurrencyExceptions = 4;
            DelayBeforeRetryOnOptimisticConcurrencyExceptions = TimeSpan.FromMilliseconds(100);
            ThrowSubscriberExceptions = false;
            IsAsynchronousSubscribersEnabled = false;
            CancellationBoundary = CancellationBoundary.BeforeCommittingEvents;
            ForwardOptimisticConcurrencyExceptions = false;
        }
    }
}