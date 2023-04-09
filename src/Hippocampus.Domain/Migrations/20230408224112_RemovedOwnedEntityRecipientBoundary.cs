using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hippocampus.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RemovedOwnedEntityRecipientBoundary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecipientBoundary_MinHeight",
                table: "RecipientMonitors",
                newName: "MinHeight");

            migrationBuilder.RenameColumn(
                name: "RecipientBoundary_MaxHeight",
                table: "RecipientMonitors",
                newName: "MaxHeight");

            migrationBuilder.AlterColumn<int>(
                name: "MinHeight",
                table: "RecipientMonitors",
                type: "integer",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<int>(
                name: "MaxHeight",
                table: "RecipientMonitors",
                type: "integer",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MinHeight",
                table: "RecipientMonitors",
                newName: "RecipientBoundary_MinHeight");

            migrationBuilder.RenameColumn(
                name: "MaxHeight",
                table: "RecipientMonitors",
                newName: "RecipientBoundary_MaxHeight");

            migrationBuilder.AlterColumn<float>(
                name: "RecipientBoundary_MinHeight",
                table: "RecipientMonitors",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<float>(
                name: "RecipientBoundary_MaxHeight",
                table: "RecipientMonitors",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
