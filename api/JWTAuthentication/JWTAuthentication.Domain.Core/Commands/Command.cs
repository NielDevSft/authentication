// The MIT License (MIT)
using JWTAuthentication.Domain.Core.Aggregates;
using JWTAuthentication.Domain.Core.Aggregates.ExecutionResults;
using JWTAuthentication.Domain.Core.Core;
using JWTAuthentication.Domain.Core.ValueObjects;

namespace JWTAuthentication.Domain.Core.Commands
{
    public abstract class Command<TAggregate, TIdentity, TExecutionResult> :
        ValueObject,
        ICommand<TAggregate, TIdentity, TExecutionResult>
        where TAggregate : IAggregateRoot<TIdentity>
        where TIdentity : IIdentity
        where TExecutionResult : IExecutionResult
    {
        public ISourceId SourceId { get; }
        public TIdentity AggregateId { get; }

        protected Command(TIdentity aggregateId)
            : this(aggregateId, CommandId.New)
        {
        }

        protected Command(TIdentity aggregateId, ISourceId sourceId)
        {
            if (aggregateId == null) throw new ArgumentNullException(nameof(aggregateId));
            if (sourceId == null) throw new ArgumentNullException(nameof(sourceId));

            AggregateId = aggregateId;
            SourceId = sourceId;
        }

        public async Task<IExecutionResult> PublishAsync(ICommandBus commandBus, CancellationToken cancellationToken)
        {
            return await commandBus.PublishAsync(this, cancellationToken).ConfigureAwait(false);
        }

        public ISourceId GetSourceId()
        {
            return SourceId;
        }
    }

    public abstract class Command<TAggregate, TIdentity> :
        Command<TAggregate, TIdentity, IExecutionResult>
        where TAggregate : IAggregateRoot<TIdentity>
        where TIdentity : IIdentity
    {
        protected Command(TIdentity aggregateId)
            : this(aggregateId, CommandId.New)
        {
        }

        protected Command(TIdentity aggregateId, ISourceId sourceId)
            : base(aggregateId, sourceId)
        {
        }
    }
}
