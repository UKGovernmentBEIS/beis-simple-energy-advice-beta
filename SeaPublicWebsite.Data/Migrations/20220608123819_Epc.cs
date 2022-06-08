using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    public partial class Epc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoursOfHeating",
                table: "PropertyData");

            migrationBuilder.AddColumn<string>(
                name: "EpcId",
                table: "PropertyData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HoursOfHeatingEvening",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HoursOfHeatingMorning",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Epc",
                columns: table => new
                {
                    EpcId = table.Column<string>(type: "text", nullable: false),
                    Address1 = table.Column<string>(type: "text", nullable: true),
                    Address2 = table.Column<string>(type: "text", nullable: true),
                    Postcode = table.Column<string>(type: "text", nullable: true),
                    BuildingReference = table.Column<string>(type: "text", nullable: true),
                    InspectionDate = table.Column<string>(type: "text", nullable: true),
                    PropertyType = table.Column<int>(type: "integer", nullable: true),
                    HeatingType = table.Column<int>(type: "integer", nullable: true),
                    WallConstruction = table.Column<int>(type: "integer", nullable: true),
                    SolidWallsInsulated = table.Column<int>(type: "integer", nullable: true),
                    CavityWallsInsulated = table.Column<int>(type: "integer", nullable: true),
                    FloorConstruction = table.Column<int>(type: "integer", nullable: true),
                    FloorInsulated = table.Column<int>(type: "integer", nullable: true),
                    ConstructionAgeBand = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Epc", x => x.EpcId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyData_EpcId",
                table: "PropertyData",
                column: "EpcId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyData_Epc_EpcId",
                table: "PropertyData",
                column: "EpcId",
                principalTable: "Epc",
                principalColumn: "EpcId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyData_Epc_EpcId",
                table: "PropertyData");

            migrationBuilder.DropTable(
                name: "Epc");

            migrationBuilder.DropIndex(
                name: "IX_PropertyData_EpcId",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "EpcId",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "HoursOfHeatingEvening",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "HoursOfHeatingMorning",
                table: "PropertyData");

            migrationBuilder.AddColumn<decimal>(
                name: "HoursOfHeating",
                table: "PropertyData",
                type: "numeric",
                nullable: true);
        }
    }
}
