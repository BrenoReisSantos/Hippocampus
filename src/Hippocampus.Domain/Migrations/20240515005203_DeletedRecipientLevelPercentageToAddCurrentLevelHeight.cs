using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hippocampus.Domain.Migrations
{
    /// <inheritdoc />
    public partial class DeletedRecipientLevelPercentageToAddCurrentLevelHeight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LevelPercentage",
                table: "RecipientMonitors",
                newName: "CurrentLevelHeight");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentLevelHeight",
                table: "RecipientMonitors",
                newName: "LevelPercentage");
        }
    }
}
