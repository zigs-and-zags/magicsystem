using Toolbox.Infrastructure;

namespace MagicSystem.Spells.Conjuration;

internal record CreateTome(string Rarity, string Name, string[] Spells) : Spell
{
    public string CanonicallyAuditedAs() => SpellType.Conjuration.CreateTome;

    public async Task Cast(ContentStore onContentStore, CancellationToken cancellation)
    {
        if (!Spells.All(spell => SpellType.All.Contains(spell))) throw new InvalidDataException("Ensure you pass valid spells");
        var tome = NewTome(withRarity: Rarity, withSpells: Spells);
        await onContentStore.Store(ContentStore.Identifier.From(Name), tome, cancellation);
    }

    private static Tome.Entry NewTome(string withRarity, string[] withSpells) => withRarity switch
    {
        "common" => CommonTome(withSpells),
        "rare" => RareTome(withSpells),
        "legendary" => LegendaryTome(withSpells),
        _ => CommonTome(withSpells)
    };
        
    private static Tome.Entry CommonTome(string[] withSpells)
        => new()
        {
            Spells = withSpells,
            Uses = 10,
            ActiveUntil = DateTimeOffset.Now.AddDays(7),
        };
    
    private static Tome.Entry RareTome(string[] withSpells)
        => new()
        {
            Spells = withSpells,
            Uses = 100,
            ActiveUntil = DateTimeOffset.Now.AddYears(1),
        };
    
    private static Tome.Entry LegendaryTome(string[] withSpells)
        => new()
        {
            Spells = withSpells,
            Uses = int.MaxValue,
            ActiveFrom = DateTimeOffset.MinValue,
            ActiveUntil = DateTimeOffset.MaxValue,
        };
}