namespace MagicSystem.Spells;

// TODO combine SpellTypes and the service which monitors Infrastructure? => function to check dependencies of spells and service health? => then add scheduling for events?
// TODO need to look into polymorphic json for a better solution, don't like this, too much manual work
internal static class SpellType
{ 
    internal static class Conjuration
    {
        internal const string CreateTome = "conjure-tome";
    }
    
    internal static IEnumerable<string> All => new List<string>
    {
        Conjuration.CreateTome,
    };

    internal static Type For(string type) => type switch
    {
        Conjuration.CreateTome => typeof(Spells.Conjuration.CreateTome),
        _ => throw new NotImplementedException()
    };
}