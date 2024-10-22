// The MIT License (MIT)
// 
// Copyright (c) 2015-2024 Rasmus Mikkelsen
// https://github.com/eventflow/EventFlow
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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