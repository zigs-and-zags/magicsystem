namespace Toolbox.Infrastructure;

// To quickly test "residuality" without infra orchestration tools
// You probably shouldn't do this in code shipping to prod, there's better ways :)
public static class Stub
{
    public sealed class VoidsAuditTrail(bool shouldCrashWhenCalled = false) : AuditTrail
    {
        public Task Store(AuditTrail.Entry entry, CancellationToken cancellation = default)
            => shouldCrashWhenCalled ? Crash : Task.CompletedTask;
        public Task<IReadOnlyCollection<AuditTrail.Entry>> For(AuditTrail.CorrelationIdentifier correlationIdentifier, CancellationToken cancellationToken)
            => (Task<IReadOnlyCollection<AuditTrail.Entry>>)(shouldCrashWhenCalled ? Crash : Task.FromResult<IReadOnlyCollection<AuditTrail.Entry>>(Array.Empty<AuditTrail.Entry>()));
    }
    
    public sealed class VoidsContentStore(bool shouldCrashWhenCalled = false) : ContentStore
    {
        public Task Delete(ContentStore.Identifier id, CancellationToken cancellation = default)
            => shouldCrashWhenCalled ? Crash : Task.CompletedTask;
        public Task<TContent?> FindBy<TContent>(
            ContentStore.Identifier id,
            CancellationToken cancellation = default) where TContent : ContentStore.Content
            => (Task<TContent?>)(shouldCrashWhenCalled ? Crash : Task.FromResult<TContent?>(default));
        public Task<bool> Exists(ContentStore.Identifier id, CancellationToken cancellation = default)
            => (Task<bool>)(shouldCrashWhenCalled ? Crash : Task.FromResult(false));
        public Task Store<TContent>(ContentStore.Identifier id,
            TContent content, CancellationToken cancellation = default) where TContent : ContentStore.Content
            => shouldCrashWhenCalled ? Crash : Task.CompletedTask;
    }
    private static Task Crash => Task.FromException(new Exception($"You just activated my trap card!"));
}