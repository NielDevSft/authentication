using JWTAuthentication.Domain.Core.ValueObjects;

namespace JWTAuthentication.Domain.Core.Aggregates
{
    public class AggregateName : SingleValueObject<string>, IAggregateName
    {
        public AggregateName(string value) : base(value)
        {
        }
    }
}