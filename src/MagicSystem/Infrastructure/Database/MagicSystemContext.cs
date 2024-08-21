using MagicSystem.Infrastructure.AuditTrail;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MagicSystem.Infrastructure.Database
{
    public class MagicSystemContext : DbContext
    {
        public MagicSystemContext(DbContextOptions<MagicSystemContext> options) : base(options) { }

        public DbSet<Toolbox.Infrastructure.AuditTrail.Entry> AuditEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("MagicSystem");
            modelBuilder.ApplyConfiguration(new DatabaseAuditTrail.Configuration());
        }

        internal class DbContextFactory : IDesignTimeDbContextFactory<MagicSystemContext>
        {
            public MagicSystemContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<MagicSystemContext>();
                builder.UseSqlite("Data Source=./Infrastructure/Database/MagicSystem.db");

                return new MagicSystemContext(builder.Options);
            }
        }
    }
}