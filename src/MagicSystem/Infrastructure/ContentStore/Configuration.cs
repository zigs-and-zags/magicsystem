using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MagicSystem.Infrastructure.ContentStore;

// TODO idea of disabling ContentStore half of the time and having a "AutoHeal" endpoint which I can call to show the system can recover

public static class Configuration
{
    internal static IServiceCollection AddContentStore(this IServiceCollection services, IConfiguration configuration)
        => configuration.GetSection("ContentStore")["Provider"] switch
        {
            "FileSystem" => services.AddFileSystemContentStore(configuration),
            "None" => services.AddSingleton<Toolbox.Infrastructure.ContentStore, Toolbox.Infrastructure.Stub.VoidsContentStore>(),
            _ => services
        };

    private static IServiceCollection AddFileSystemContentStore(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddOptions()
            .Configure<FileSystemStorageOptions>(options => configuration.GetSection("ContentStore:FileSystemStorageOptions").Bind(options))
            .AddScoped<Toolbox.Infrastructure.ContentStore, FileSystemContentStore>();
    
    public record FileSystemStorageOptions
    {
        public string MountOnDirectoryPath { get; set; }
    };
}