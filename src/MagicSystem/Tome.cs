using Toolbox.Infrastructure;

namespace MagicSystem;

internal static class Tome
{
    private static readonly Random Random = new();
    private const string Glyphs = "!@#$%^*()_-={}[]|,.~";
    // private const string Runes = "ᚠᚢᚦᚫᚱᚲᚷᚹᚺᚾᛁᛃᛇᛈᛉᛋᛏᛒᛖᛗᛚᛝᛞᛟ"; // TODO change json serializer and use these
    private static string RandomGlyphs(int withLength) =>
        new(Enumerable
            .Repeat($"{Glyphs}", withLength)
            .Select(x => x[Random.Next(x.Length)])
            .ToArray());

    // TODO SCD on Entry.Active[From/Until]?
    internal record Entry : ContentStore.Content
    {
        public required string[] Spells { get; init; }
        public int Uses { get; init; } = 1;
        public DateTimeOffset ActiveFrom { get; init; } = DateTimeOffset.Now;
        public DateTimeOffset ActiveUntil { get; init; } = DateTimeOffset.Now.AddMinutes(5);
        public AuditTrail.CorrelationIdentifier Title { get; private init; } = new(RandomGlyphs(withLength: 16));
    }
}