using JWTAuthentication.Domain.Core.Aggregates;
using JWTAuthentication.Domain.Core.Aggregates.ExecutionResults;
using JWTAuthentication.Domain.Core.Commands;
using JWTAuthentication.Domain.Core.Core;

namespace JWTAuthentication.Domain.Core
{
    public interface ICommandBus
    {
        Task<TExecutionResult> PublishAsync<TAggregate, TIdentity, TExecutionResult>(
            ICommand<TAggregate, TIdentity, TExecutionResult> command,
            CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity
            where TExecutionResult : IExecutionResult;
    }
}
