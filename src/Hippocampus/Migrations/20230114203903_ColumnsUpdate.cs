using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hippocampus.Migrations
{
    /// <inheritdoc />
    public partial class ColumnsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecipientLogs_MacAddress",
                table: "RecipientLogs");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "RecipientLogs");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "RecipientLogs",
                newName: "RecipientLogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecipientLogId",
                table: "RecipientLogs",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "RecipientLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RecipientLogs_MacAddress",
                table: "RecipientLogs",
                column: "MacAddress",
                unique: true);
        }
    }
}
