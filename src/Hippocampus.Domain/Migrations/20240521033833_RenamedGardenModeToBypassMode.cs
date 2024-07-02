using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hippocampus.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RenamedGardenModeToBypassMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GardenMode",
                table: "WaterTank",
                newName: "BypassMode"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BypassMode",
                table: "WaterTank",
                newName: "GardenMode"
            );
        }
    }
}
