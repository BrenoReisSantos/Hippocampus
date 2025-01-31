﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable IDE0161
namespace Hippocampus.Domain.Migrations
#pragma warning restore IDE0161
{
    /// <inheritdoc />
    public partial class AddedGardenModeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GardenMode",
                table: "WaterTankLog",
                type: "boolean",
                nullable: true
            );

            migrationBuilder.AddColumn<bool>(
                name: "GardenMode",
                table: "WaterTank",
                type: "boolean",
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "GardenMode", table: "WaterTankLog");

            migrationBuilder.DropColumn(name: "GardenMode", table: "WaterTank");
        }
    }
}
