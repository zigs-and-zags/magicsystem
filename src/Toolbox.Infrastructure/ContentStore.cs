namespace Toolbox.Infrastructure;

// TODO use as storage for outbox
public interface ContentStore
{
    public interface Content { }
    // I really like strongly typed id's for readability, it can also hide some specific formatting things, etc ...
    // This comes at a tradeoff of having an object as id, so you have to parse somewhere, especially for read models
    // A readonly record struct is ideal for these objects, only use this for small data structures where you don't want to compare "by ref"
    public readonly record struct Identifier(string Value)
    {
        public static Identifier From(string value) => new(value);
        public override string ToString() => Value.ToString();
    }
    
    
    public Task Store<TContent>(
        ContentStore.Identifier id,
        TContent content,
        CancellationToken cancellation) where TContent : Content;
    public Task<TContent?> FindBy<TContent>(ContentStore.Identifier id, CancellationToken cancellation) where TContent : Content;
    public Task<bool> Exists(ContentStore.Identifier id, CancellationToken cancellation);
    public Task Delete(ContentStore.Identifier id, CancellationToken cancellation);
}