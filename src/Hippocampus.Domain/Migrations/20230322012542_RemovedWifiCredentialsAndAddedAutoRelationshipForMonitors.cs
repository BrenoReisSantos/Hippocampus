using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hippocampus.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RemovedWifiCredentialsAndAddedAutoRelationshipForMonitors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WifiPassword",
                table: "RecipientMonitors");

            migrationBuilder.DropColumn(
                name: "WifiSsid",
                table: "RecipientMonitors");

            migrationBuilder.AddColumn<Guid>(
                name: "MonitorLinkedToRecipientMonitorId",
                table: "RecipientMonitors",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RecipientMonitors_MonitorLinkedToRecipientMonitorId",
                table: "RecipientMonitors",
                column: "MonitorLinkedToRecipientMonitorId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientMonitors_RecipientMonitors_MonitorLinkedToRecipien~",
                table: "RecipientMonitors",
                column: "MonitorLinkedToRecipientMonitorId",
                principalTable: "RecipientMonitors",
                principalColumn: "RecipientMonitorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipientMonitors_RecipientMonitors_MonitorLinkedToRecipien~",
                table: "RecipientMonitors");

            migrationBuilder.DropIndex(
                name: "IX_RecipientMonitors_MonitorLinkedToRecipientMonitorId",
                table: "RecipientMonitors");

            migrationBuilder.DropColumn(
                name: "MonitorLinkedToRecipientMonitorId",
                table: "RecipientMonitors");

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
    }
}
