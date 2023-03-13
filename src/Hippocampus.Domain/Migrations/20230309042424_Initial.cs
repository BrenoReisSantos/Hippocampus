using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hippocampus.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecipientMonitors",
                columns: table => new
                {
                    RecipientMonitorId = table.Column<Guid>(type: "uuid", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RecipientBoundaryMinHeight = table.Column<float>(name: "RecipientBoundary_MinHeight", type: "real", nullable: false),
                    RecipientBoundaryMaxHeight = table.Column<float>(name: "RecipientBoundary_MaxHeight", type: "real", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipientMonitors", x => x.RecipientMonitorId);
                });

            migrationBuilder.CreateTable(
                name: "RecipientLogs",
                columns: table => new
                {
                    RecipientLogId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    State = table.Column<string>(type: "text", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RecipientMonitorId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipientLogs", x => x.RecipientLogId);
                    table.UniqueConstraint("AK_RecipientLogs_RecipientMonitorId_RegisterDate", x => new { x.RecipientMonitorId, x.RegisterDate });
                    table.ForeignKey(
                        name: "FK_RecipientLogs_RecipientMonitors_RecipientMonitorId",
                        column: x => x.RecipientMonitorId,
                        principalTable: "RecipientMonitors",
                        principalColumn: "RecipientMonitorId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipientLogs");

            migrationBuilder.DropTable(
                name: "RecipientMonitors");
        }
    }
}
