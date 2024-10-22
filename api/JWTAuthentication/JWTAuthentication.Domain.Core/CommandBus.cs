using JWTAuthentication.Domain.Core.Aggregates;
using JWTAuthentication.Domain.Core.Aggregates.ExecutionResults;
using JWTAuthentication.Domain.Core.Commands;
using JWTAuthentication.Domain.Core.Core;
using JWTAuthentication.Domain.Core.Core.Caching;
using JWTAuthentication.Domain.Core.Exceptions;
using JWTAuthentication.Domain.Core.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace JWTAuthentication.Domain.Core
{
    public class CommandBus : ICommandBus
    {
        private readonly ILogger<CommandBus> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAggregateStore _aggregateStore;
        private readonly IMemoryCache _memoryCache;

        public CommandBus(
            ILogger<CommandBus> logger,
            IServiceProvider serviceProvider,
            IAggregateStore aggregateStore,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _aggregateStore = aggregateStore;
            _memoryCache = memoryCache;
        }

        public async Task<TResult> PublishAsync<TAggregate, TIdentity, TResult>(
            ICommand<TAggregate, TIdentity, TResult> command,
            CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity
            where TResult : IExecutionResult
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace(
                    "Executing command {CommandType} with ID {CommandId} on aggregate {AggregateType}",
                    command.GetType().PrettyPrint(),
                    command.SourceId,
                    typeof(TAggregate).PrettyPrint());
            }

            var aggregateUpdateResult = await ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

            if (_logger.IsEnabled(LogLevel.Trace))
            {
                if (aggregateUpdateResult.DomainEvents.Any())
                {
                    _logger.LogTrace(
                        "Execution command {CommandType} with ID {CommandId} on aggregate {AggregateType} did NOT result in any domain events, was success: {IsSuccess}",
                        command.GetType().PrettyPrint(),
                        command.SourceId,
                        typeof(TAggregate).PrettyPrint(),
                        aggregateUpdateResult.Result?.IsSuccess);
                }
                else
                {
                    _logger.LogTrace(
                        "Execution command {CommandType} with ID {CommandId} on aggregate {AggregateType} resulted in these events: {EventTypes}, was success: {IsSuccess}",
                        command.GetType().PrettyPrint(),
                        command.SourceId,
                        typeof(TAggregate).PrettyPrint(),
                        aggregateUpdateResult.DomainEvents.Select(d => d.EventType.PrettyPrint()).ToList(),
                        aggregateUpdateResult.Result?.IsSuccess);
                }
            }

            return aggregateUpdateResult.Result;
        }

        private async Task<IAggregateUpdateResult<TResult>> ExecuteCommandAsync<TAggregate, TIdentity, TResult>(
            ICommand<TAggregate, TIdentity, TResult> command,
            CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity
            where TResult : IExecutionResult
        {
            var commandType = command.GetType();
            var commandExecutionDetails = GetCommandExecutionDetails(commandType);

            var commandHandlers = _serviceProvider.GetServices(commandExecutionDetails.CommandHandlerType)
                .Cast<ICommandHandler>()
                .ToList();
            if (!commandHandlers.Any())
            {
                throw new NoCommandHandlersException(string.Format(
                    "No command handlers registered for the command '{0}' on aggregate '{1}'",
                    commandType.PrettyPrint(),
                    typeof(TAggregate).PrettyPrint()));
            }
            if (commandHandlers.Count > 1)
            {
                throw new InvalidOperationException(string.Format(
                    "Too many command handlers the command '{0}' on aggregate '{1}'. These were found: {2}",
                    commandType.PrettyPrint(),
                    typeof(TAggregate).PrettyPrint(),
                    string.Join(", ", commandHandlers.Select(h => h.GetType().PrettyPrint()))));
            }

            var commandHandler = commandHandlers.Single();

            return await _aggregateStore.UpdateAsync<TAggregate, TIdentity, TResult>(
                command.AggregateId,
                command.SourceId,
                (a, c) => (Task<TResult>)commandExecutionDetails.Invoker(commandHandler, a, command, c),
                cancellationToken)
                .ConfigureAwait(false);
        }

        private class CommandExecutionDetails
        {
            public Type CommandHandlerType { get; set; }
            public Func<ICommandHandler, IAggregateRoot, ICommand, CancellationToken, Task> Invoker { get; set; }
        }

        private const string NameOfExecuteCommand = nameof(
            ICommandHandler<
                IAggregateRoot<IIdentity>,
                IIdentity,
                IExecutionResult,
                ICommand<IAggregateRoot<IIdentity>, IIdentity, IExecutionResult>
            >.ExecuteCommandAsync);
        private CommandExecutionDetails GetCommandExecutionDetails(Type commandType)
        {
            return _memoryCache.GetOrCreate(
                CacheKey.With(GetType(), commandType.GetCacheKey()),
                e =>
                {
                    e.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
                    var commandInterfaceType = commandType
                        .GetTypeInfo()
                        .GetInterfaces()
                        .Single(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<,,>));
                    var commandTypes = commandInterfaceType.GetTypeInfo().GetGenericArguments();

                    var commandHandlerType = typeof(ICommandHandler<,,,>)
                        .MakeGenericType(commandTypes[0], commandTypes[1], commandTypes[2], commandType);

                    _logger.LogDebug(
                        "Command {CommandType} is resolved by {CommandHandlerType}",
                        commandType.PrettyPrint(),
                        commandHandlerType.PrettyPrint());

                    var invokeExecuteAsync = ReflectionHelper.CompileMethodInvocation<Func<ICommandHandler, IAggregateRoot, ICommand, CancellationToken, Task>>(
                        commandHandlerType, NameOfExecuteCommand);

                    return new CommandExecutionDetails
                    {
                        CommandHandlerType = commandHandlerType,
                        Invoker = invokeExecuteAsync
                    };
                });
        }
    }
}
