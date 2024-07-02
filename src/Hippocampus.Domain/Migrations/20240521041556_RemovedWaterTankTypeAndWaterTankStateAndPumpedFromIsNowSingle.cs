using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hippocampus.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RemovedWaterTankTypeAndWaterTankStateAndPumpedFromIsNowSingle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_WaterTank_PumpsToWaterTankId", table: "WaterTank");

            migrationBuilder.DropColumn(name: "State", table: "WaterTankLog");

            migrationBuilder.DropColumn(name: "State", table: "WaterTank");

            migrationBuilder.DropColumn(name: "Type", table: "WaterTank");

            migrationBuilder.RenameColumn(
                name: "GardenMode",
                table: "WaterTankLog",
                newName: "BypassMode"
            );

            migrationBuilder.CreateIndex(
                name: "IX_WaterTank_PumpsToWaterTankId",
                table: "WaterTank",
                column: "PumpsToWaterTankId",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_WaterTank_PumpsToWaterTankId", table: "WaterTank");

            migrationBuilder.RenameColumn(
                name: "BypassMode",
                table: "WaterTankLog",
                newName: "GardenMode"
            );

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "WaterTankLog",
                type: "text",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "WaterTank",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "WaterTank",
                type: "text",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.CreateIndex(
                name: "IX_WaterTank_PumpsToWaterTankId",
                table: "WaterTank",
                column: "PumpsToWaterTankId"
            );
        }
    }
}
