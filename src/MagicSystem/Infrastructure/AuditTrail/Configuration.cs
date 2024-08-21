using MagicSystem.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MagicSystem.Infrastructure.AuditTrail;

public static class Configuration
{
    internal static IServiceCollection AddAuditTrail(this IServiceCollection services, IConfiguration configuration)
        => configuration.GetSection("AuditTrail")["Provider"] switch
        {
            "Sqlite" => services
                .AddScoped<Toolbox.Infrastructure.AuditTrail, DatabaseAuditTrail>()
                .AddDbContext<MagicSystemContext>(options => options.UseSqlite(configuration.GetSection("AuditTrail:Sqlite")["DatabaseFilePath"])),
            "None" => services.AddSingleton<Toolbox.Infrastructure.AuditTrail, Toolbox.Infrastructure.Stub.VoidsAuditTrail>(),
            _ => services
        };
}