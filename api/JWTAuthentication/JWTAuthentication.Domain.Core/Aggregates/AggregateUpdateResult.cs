using JWTAuthentication.Domain.Core.Aggregates.ExecutionResults;

namespace JWTAuthentication.Domain.Core.Aggregates
{
    public interface IAggregateUpdateResult<out TExecutionResult>
        where TExecutionResult : IExecutionResult
    {
        TExecutionResult Result { get; }
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    }
}