namespace JWTAuthentication.Domain.Core.Aggregates.ExecutionResults
{
    public interface IExecutionResult
    {
        bool IsSuccess { get; }
    }
}