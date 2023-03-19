using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hippocampus.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddedWifiInfoAndRecipientType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipientType",
                table: "RecipientMonitors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WifiPassword",
                table: "RecipientMonitors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WifiSsid",
                table: "RecipientMonitors",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientType",
                table: "RecipientMonitors");

            migrationBuilder.DropColumn(
                name: "WifiPassword",
                table: "RecipientMonitors");

            migrationBuilder.DropColumn(
                name: "WifiSsid",
                table: "RecipientMonitors");
        }
    }
}
