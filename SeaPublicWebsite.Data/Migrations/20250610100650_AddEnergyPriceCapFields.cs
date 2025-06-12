using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEnergyPriceCapFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnergyPriceCapInfoRequested",
                table: "PropertyData",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EnergyPriceCapMonthIndex",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnergyPriceCapYear",
                table: "PropertyData",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnergyPriceCapInfoRequested",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "EnergyPriceCapMonthIndex",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "EnergyPriceCapYear",
                table: "PropertyData");
        }
    }
}
