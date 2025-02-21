using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    public partial class AddHasSolarElectricPanelFieldToEPC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HasSolarElectricPanels",
                table: "Epc",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasSolarElectricPanels",
                table: "Epc");
        }
    }
}
