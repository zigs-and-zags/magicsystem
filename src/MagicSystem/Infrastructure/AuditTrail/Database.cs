using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;
using MagicSystem.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MagicSystem.Infrastructure.AuditTrail;
using Toolbox.Infrastructure;

internal class DatabaseAuditTrail(MagicSystemContext db) : AuditTrail
{
    public async Task Store(AuditTrail.Entry entry, CancellationToken cancellationToken)
    {
        await db.AuditEvents.AddAsync(entry, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<AuditTrail.Entry>> For(AuditTrail.CorrelationIdentifier correlationIdentifier, CancellationToken cancellationToken)
        => await db.AuditEvents
            .AsNoTracking()
            .Where(x => x.CorrelationIdentifier == correlationIdentifier)
            .ToListAsync(cancellationToken);
    
    internal class Configuration : IEntityTypeConfiguration<AuditTrail.Entry>
    {
        public void Configure(EntityTypeBuilder<AuditTrail.Entry> builder)
        {
            builder.ToTable("AuditedEvents");
            builder.HasKey(e => e.Identifier);
            builder.HasIndex(e => e.CorrelationIdentifier);

            builder
                .Property(e => e.Identifier)
                .HasConversion(
                    id => id.Value,
                    value => new AuditTrail.Identifier(value))
                .IsRequired();

            builder
                .Property(e => e.CanonicalType)
                .IsRequired()
                .HasMaxLength(255);

            builder
                .Property(e => e.Timestamp)
                .IsRequired();

            builder
                .Property(e => e.ExtraContext)
                .HasConversion(
                    context => JsonSerializer.Serialize(context, JsonParsingOptions),
                    json => (JsonSerializer
                        .Deserialize<Dictionary<string, string>>(json, JsonParsingOptions) ?? new Dictionary<string, string>())
                        .ToFrozenDictionary(null))
                .HasColumnType("TEXT");

            builder
                .Property(e => e.CorrelationIdentifier)
                .HasConversion(
                    id => id.Value,
                    value => new AuditTrail.CorrelationIdentifier(value))
                .IsRequired();
        }
        
        private static readonly JsonSerializerOptions JsonParsingOptions = new()
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
    }
}