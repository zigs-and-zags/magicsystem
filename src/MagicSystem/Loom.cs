using Toolbox.Infrastructure;

namespace MagicSystem;

// "Loom" because "Weave" was already taken :)
internal class Loom(AuditTrail auditTrail, ContentStore contentStore)
{
    // TODO outbox implementation
    // TODO check if tome can cast the spell (in spells, still has uses, is still active)
    internal async Task Cast(AuditTrail.CorrelationIdentifier tomeTitle, Spell spell, CancellationToken cancellation)
    {
        await auditTrail.Store(AuditTrail.Entry.Create(new Cast.Event.Initiated(spell), tomeTitle), cancellation);
        
        Exception? madeABooBoo = null;
        try { await TaskFor(spell, cancellation); }
        catch (Exception? exception) { madeABooBoo = exception; }

        await auditTrail.Store((madeABooBoo is null)
            ? AuditTrail.Entry.Create(new Cast.Event.Completed(spell), tomeTitle)
            : AuditTrail.Entry
                .Create(new Cast.Event.Completed(spell, IsSuccessful: false), tomeTitle)
                .WithExtraContext(new Dictionary<string, string> { { "exception", madeABooBoo.ToString() } }), cancellation);
    }

    internal async Task<IReadOnlyCollection<AuditTrail.Entry>> History(AuditTrail.CorrelationIdentifier tomeTitle, CancellationToken cancellation)
        => await auditTrail.For(tomeTitle, cancellation);
        
    // TODO Healthcheck

    private Task TaskFor(Spell spell, CancellationToken cancellation) => spell switch
    {
        Spells.Conjuration.CreateTome x => x.Cast(onContentStore: contentStore, cancellation: cancellation),
        _ => Task.FromException(new NotImplementedException())
    };
}