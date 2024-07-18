using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable IDE0161
namespace Hippocampus.Domain.Migrations
#pragma warning restore IDE0161
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
