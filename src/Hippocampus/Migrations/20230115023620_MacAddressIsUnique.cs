using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hippocampus.Migrations
{
    /// <inheritdoc />
    public partial class MacAddressIsUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RecipientLogs_MacAddress",
                table: "RecipientLogs",
                column: "MacAddress",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecipientLogs_MacAddress",
                table: "RecipientLogs");
        }
    }
}
