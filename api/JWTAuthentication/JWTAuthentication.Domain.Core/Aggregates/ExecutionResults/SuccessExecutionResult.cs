namespace JWTAuthentication.Domain.Core.Aggregates.ExecutionResults
{
    public class SuccessExecutionResult : ExecutionResult
    {
        public override bool IsSuccess { get; } = true;

        public override string ToString()
        {
            return "Successful execution";
        }
    }
}