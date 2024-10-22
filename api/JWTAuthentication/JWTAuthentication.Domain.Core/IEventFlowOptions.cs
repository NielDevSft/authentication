using JWTAuthentication.Domain.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JWTAuthentication.Domain.Core
{
    public interface IEventFlowOptions
    {
        IServiceCollection ServiceCollection { get; }

        IEventFlowOptions ConfigureThrowSubscriberExceptions(bool shouldThrow);
        IEventFlowOptions ConfigureOptimisticConcurrencyRetry(int retries, TimeSpan delayBeforeRetry);
        IEventFlowOptions Configure(Action<EventFlowConfiguration> configure);
        IEventFlowOptions AddEvents(IEnumerable<Type> aggregateEventTypes);
        IEventFlowOptions AddCommands(IEnumerable<Type> commandTypes);
        IEventFlowOptions AddJobs(IEnumerable<Type> jobTypes);
        IEventFlowOptions AddSagas(IEnumerable<Type> sagaTypes);
        IEventFlowOptions AddSnapshots(IEnumerable<Type> snapshotTypes);
    }
}