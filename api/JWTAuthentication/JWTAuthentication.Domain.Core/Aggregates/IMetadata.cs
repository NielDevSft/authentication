using JWTAuthentication.Domain.Core.Core;

namespace JWTAuthentication.Domain.Core.Aggregates
{
    public interface IMetadata : IMetadataContainer
    {
        IEventId EventId { get; }
        ISourceId SourceId { get; }
        string EventName { get; }
        int EventVersion { get; }
        DateTimeOffset Timestamp { get; }
        long TimestampEpoch { get; }
        int AggregateSequenceNumber { get; }
        string AggregateId { get; }
        string AggregateName { get; }

        IMetadata CloneWith(params KeyValuePair<string, string>[] keyValuePairs);
        IMetadata CloneWith(IEnumerable<KeyValuePair<string, string>> keyValuePairs);
    }
}
