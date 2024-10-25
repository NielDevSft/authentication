using JWTAuthentication.Domain.Core.Aggregates;
using JWTAuthentication.Domain.Core.Aggregates.ExecutionResults;
using JWTAuthentication.Domain.Core.Core;

namespace JWTAuthentication.Domain.Core.Commands
{
    public interface ICommandHandler
    {
    }

    public interface ICommandHandler<in TAggregate, TIdentity, TResult, in TCommand> : ICommandHandler
        where TAggregate : IAggregateRoot<TIdentity>
        where TIdentity : IIdentity
        where TResult : IExecutionResult
        where TCommand : ICommand<TAggregate, TIdentity, TResult>
    {
        Task<TResult> ExecuteCommandAsync(TAggregate aggregate, TCommand command, CancellationToken cancellationToken);
    }
}