namespace MagicSystem.Infrastructure.ContentStore;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Toolbox.Infrastructure;

// TODO other file types
public class FileSystemContentStore : ContentStore
{
    private readonly string mounted;

    public FileSystemContentStore(IOptions<Configuration.FileSystemStorageOptions> configuration)
    {
        if (!Directory.Exists(configuration.Value.MountOnDirectoryPath))
        {
            try { Directory.CreateDirectory(configuration.Value.MountOnDirectoryPath); }
            catch (UnauthorizedAccessException _) { throw new InvalidOperationException($"Insufficient filesystem privileges for {configuration.Value.MountOnDirectoryPath}"); }
        }
        mounted = configuration.Value.MountOnDirectoryPath;
    }

    public async Task Store<TContent>(ContentStore.Identifier id, TContent content, CancellationToken cancellation) where TContent : ContentStore.Content
    {
        var path = SanitizedFilePathFor(id);
        await File.WriteAllBytesAsync(path, BinaryData.FromObjectAsJson(content).ToArray(), cancellation);
    }

    public async Task<TContent?> FindBy<TContent>(ContentStore.Identifier id, CancellationToken cancellation) where TContent : ContentStore.Content
    {
        var path = SanitizedFilePathFor(id);
        if (!File.Exists(path)) return default;
        
        var bytes = await File.ReadAllBytesAsync(path, cancellation);
        return new BinaryData(bytes).ToObjectFromJson<TContent>();
    }

    public async Task<bool> Exists(ContentStore.Identifier id, CancellationToken cancellation)
        => await Task.FromResult(File.Exists(SanitizedFilePathFor(id)));

    public Task Delete(ContentStore.Identifier id, CancellationToken cancellation)
    {
        var path = SanitizedFilePathFor(id);
        if (File.Exists(path)) File.Delete(path);
        return Task.CompletedTask;
    }
    
    private string SanitizedFilePathFor(ContentStore.Identifier id)
        => Path.Combine(mounted, $"{Path.GetFileNameWithoutExtension(id.Value)}.json");
}