using Toolbox.Infrastructure;

namespace MagicSystem;

// TODO generics implementation for casting on infra
internal interface Spell : AuditTrail.AuditedEvent { }

internal static class Cast
{
    internal static class State // TODO will be used later on
    {
        internal const string Initiated = "initiated";
        internal const string Success = "casted";
        internal const string Failed = "failed";
    }
        
    internal static class Event
    {
        internal record Initiated(Spell Spell) : AuditTrail.AuditedEvent
            { public string CanonicallyAuditedAs() => $"{State.Initiated}-{Spell.CanonicallyAuditedAs()}"; }
        
        internal record Completed(Spell Spell, bool IsSuccessful = true) : AuditTrail.AuditedEvent
            { public string CanonicallyAuditedAs() => $"{(IsSuccessful ? State.Success : State.Failed)}-{Spell.CanonicallyAuditedAs()}"; }
    }
}
