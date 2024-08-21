using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicSystem.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MagicSystem");

            migrationBuilder.CreateTable(
                name: "AuditedEvents",
                schema: "MagicSystem",
                columns: table => new
                {
                    Identifier = table.Column<string>(type: "TEXT", nullable: false),
                    CorrelationIdentifier = table.Column<string>(type: "TEXT", nullable: false),
                    CanonicalType = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ExtraContext = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditedEvents", x => x.Identifier);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditedEvents_CorrelationIdentifier",
                schema: "MagicSystem",
                table: "AuditedEvents",
                column: "CorrelationIdentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditedEvents",
                schema: "MagicSystem");
        }
    }
}
