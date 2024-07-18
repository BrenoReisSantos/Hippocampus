using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable IDE0161
namespace Hippocampus.Domain.Migrations
#pragma warning restore IDE0161
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WaterTank",
                columns: table => new
                {
                    WaterTankId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    Type = table.Column<string>(type: "text", nullable: false),
                    CurrentLevel = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    LevelWhenEmpty = table.Column<int>(type: "integer", nullable: false),
                    LevelWhenFull = table.Column<int>(type: "integer", nullable: false),
                    PumpingWater = table.Column<bool>(type: "boolean", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    PumpsToWaterTankId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterTank", x => x.WaterTankId);
                    table.ForeignKey(
                        name: "FK_WaterTank_WaterTank_PumpsToWaterTankId",
                        column: x => x.PumpsToWaterTankId,
                        principalTable: "WaterTank",
                        principalColumn: "WaterTankId"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "WaterTankLog",
                columns: table => new
                {
                    WaterTankLogId = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    PumpingWater = table.Column<bool>(type: "boolean", nullable: true),
                    LogDate = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    WaterTankId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterTankLog", x => x.WaterTankLogId);
                    table.UniqueConstraint(
                        "AK_WaterTankLog_WaterTankId_LogDate",
                        x => new { x.WaterTankId, x.LogDate }
                    );
                    table.ForeignKey(
                        name: "FK_WaterTankLog_WaterTank_WaterTankId",
                        column: x => x.WaterTankId,
                        principalTable: "WaterTank",
                        principalColumn: "WaterTankId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_WaterTank_PumpsToWaterTankId",
                table: "WaterTank",
                column: "PumpsToWaterTankId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_WaterTankLog_LogDate",
                table: "WaterTankLog",
                column: "LogDate",
                descending: new bool[0]
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "WaterTankLog");

            migrationBuilder.DropTable(name: "WaterTank");
        }
    }
}
