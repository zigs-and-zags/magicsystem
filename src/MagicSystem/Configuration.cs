using MagicSystem.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MagicSystem;

internal static class Configuration
{
    // TODO Preconfigured tomes from json file?
    internal static IServiceCollection InjectMagicSystem(
        this IServiceCollection services,
        IConfigurationRoot configuration)
        => services.AddInfraStructure(configuration).AddScoped<Loom>();
}