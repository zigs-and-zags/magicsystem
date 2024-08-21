using MagicSystem.Infrastructure.AuditTrail;
using MagicSystem.Infrastructure.ContentStore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MagicSystem.Infrastructure;

internal static class Configuration
{
    internal static IServiceCollection AddInfraStructure(
        this IServiceCollection services,
        IConfigurationRoot configuration)
        => services.AddOptions()
            .AddContentStore(configuration)
            .AddAuditTrail(configuration);
}
