using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hippocampus.Domain.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTableNameToRecipientBoundary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecipientLevelLimit_MinHeight",
                table: "RecipientMonitor",
                newName: "RecipientBoundary_MinHeight");

            migrationBuilder.RenameColumn(
                name: "RecipientLevelLimit_MaxHeight",
                table: "RecipientMonitor",
                newName: "RecipientBoundary_MaxHeight");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecipientBoundary_MinHeight",
                table: "RecipientMonitor",
                newName: "RecipientLevelLimit_MinHeight");

            migrationBuilder.RenameColumn(
                name: "RecipientBoundary_MaxHeight",
                table: "RecipientMonitor",
                newName: "RecipientLevelLimit_MaxHeight");
        }
    }
}
