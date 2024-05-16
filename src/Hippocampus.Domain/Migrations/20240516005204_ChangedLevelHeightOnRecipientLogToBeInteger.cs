using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hippocampus.Domain.Migrations
{
    /// <inheritdoc />
    public partial class ChangedLevelHeightOnRecipientLogToBeInteger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LevelPercentage",
                table: "RecipientLogs",
                newName: "LevelHeight");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LevelHeight",
                table: "RecipientLogs",
                newName: "LevelPercentage");
        }
    }
}
