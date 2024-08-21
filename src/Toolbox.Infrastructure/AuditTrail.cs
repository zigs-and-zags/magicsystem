using System.Collections.Frozen;

namespace Toolbox.Infrastructure;

public interface AuditTrail
{
    // Notice: The "AuditTrail." prepended to "Entry" seems excessive.
    // It explicitly forces you to write the full "AuditTrail.Entry" when trying to implement it.
    // Which is great when using generic-ish names inside interfaces without your code becoming spaghetti.
    Task Store(AuditTrail.Entry entry, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<AuditTrail.Entry>> For(AuditTrail.CorrelationIdentifier correlationIdentifier, CancellationToken cancellationToken);

    
    public readonly record struct Identifier(string Value);
    public readonly record struct CorrelationIdentifier(string Value);
    public interface AuditedEvent { string CanonicallyAuditedAs(); }

    // This is how you can force a factory pattern implementation in dotnet :)
    // Makes sense here, because we don't want to store too much data in these entries.
    // Made in such a way to dissuade the use of "ExtraContext"
    public record Entry
    {
        public Identifier Identifier { get; }
        public CorrelationIdentifier CorrelationIdentifier { get; }
        public string CanonicalType { get; } // Keep it the consistent over the lifetime of the system, avoid using reflection because types change
        public DateTimeOffset Timestamp { get; }
        public FrozenDictionary<string, string> ExtraContext { get; private init; }

        public static Entry Create(AuditedEvent fromAuditedEvent, CorrelationIdentifier correlateWith)
            => new(
                identifier: new Identifier(Guid.NewGuid().ToString()), // TODO check out new guid type with built-in timestamp sometime
                correlationIdentifier: correlateWith,
                canonicalType: fromAuditedEvent.CanonicallyAuditedAs(),
                timestamp: DateTimeOffset.Now);

        // Only use this when it really matters, you should avoid reading from these values in code
        // A good use case is populating extra data for the ability to replay/heal events
        public Entry WithExtraContext(IEnumerable<KeyValuePair<string, string>> context)
            => this with { ExtraContext = ExtraContext.Union(context).ToFrozenDictionary() };

        private Entry(
            Identifier identifier,
            CorrelationIdentifier correlationIdentifier,
            string canonicalType,
            DateTimeOffset timestamp)
            {
                Identifier = identifier;
                CorrelationIdentifier = correlationIdentifier;
                CanonicalType = canonicalType;
                Timestamp = timestamp;
                ExtraContext = FrozenDictionary<string, string>.Empty;
            }
    }
}