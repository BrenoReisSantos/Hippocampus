using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hippocampus.Domain.Migrations
{
    /// <inheritdoc />
    public partial class NullableLinkedMonitor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipientMonitors_RecipientMonitors_MonitorLinkedToRecipien~",
                table: "RecipientMonitors");

            migrationBuilder.AlterColumn<Guid>(
                name: "MonitorLinkedToRecipientMonitorId",
                table: "RecipientMonitors",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientMonitors_RecipientMonitors_MonitorLinkedToRecipien~",
                table: "RecipientMonitors",
                column: "MonitorLinkedToRecipientMonitorId",
                principalTable: "RecipientMonitors",
                principalColumn: "RecipientMonitorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipientMonitors_RecipientMonitors_MonitorLinkedToRecipien~",
                table: "RecipientMonitors");

            migrationBuilder.AlterColumn<Guid>(
                name: "MonitorLinkedToRecipientMonitorId",
                table: "RecipientMonitors",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientMonitors_RecipientMonitors_MonitorLinkedToRecipien~",
                table: "RecipientMonitors",
                column: "MonitorLinkedToRecipientMonitorId",
                principalTable: "RecipientMonitors",
                principalColumn: "RecipientMonitorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
