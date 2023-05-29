using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hippocampus.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RecipientLogOwnedEntityLevelPercentage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LevelPercentage",
                table: "RecipientLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LevelPercentage",
                table: "RecipientLogs");
        }
    }
}
