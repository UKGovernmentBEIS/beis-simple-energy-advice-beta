using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    public partial class PropertyDataAndRecommendations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PropertyData",
                newName: "PropertyDataId");

            migrationBuilder.AddColumn<int>(
                name: "AccessibleLoftSpace",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BungalowType",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CavityWallsInsulated",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Country",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EpcLmkKey",
                table: "PropertyData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlatType",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FloorConstruction",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FloorInsulated",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GlazingType",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HasHotWaterCylinder",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HasOutdoorSpace",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HeatingPattern",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HeatingType",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HoursOfHeating",
                table: "PropertyData",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HouseNameOrNumber",
                table: "PropertyData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HouseType",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfOccupants",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OtherHeatingType",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnershipStatus",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Postcode",
                table: "PropertyData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PropertyType",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "PropertyData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoofConstruction",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoofInsulated",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SolidWallsInsulated",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Temperature",
                table: "PropertyData",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WallConstruction",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearBuilt",
                table: "PropertyData",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PropertyRecommendations",
                columns: table => new
                {
                    PropertyRecommendationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<int>(type: "integer", nullable: false),
                    MinInstallCost = table.Column<int>(type: "integer", nullable: false),
                    MaxInstallCost = table.Column<int>(type: "integer", nullable: false),
                    Saving = table.Column<int>(type: "integer", nullable: false),
                    LifetimeSaving = table.Column<int>(type: "integer", nullable: false),
                    Lifetime = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Summary = table.Column<string>(type: "text", nullable: true),
                    RecommendationAction = table.Column<int>(type: "integer", nullable: true),
                    PropertyDataId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyRecommendations", x => x.PropertyRecommendationId);
                    table.ForeignKey(
                        name: "FK_PropertyRecommendations_PropertyData_PropertyDataId",
                        column: x => x.PropertyDataId,
                        principalTable: "PropertyData",
                        principalColumn: "PropertyDataId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyRecommendations_PropertyDataId",
                table: "PropertyRecommendations",
                column: "PropertyDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyRecommendations");

            migrationBuilder.DropColumn(
                name: "AccessibleLoftSpace",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "BungalowType",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "CavityWallsInsulated",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "EpcLmkKey",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "FlatType",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "FloorConstruction",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "FloorInsulated",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "GlazingType",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "HasHotWaterCylinder",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "HasOutdoorSpace",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "HeatingPattern",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "HeatingType",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "HoursOfHeating",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "HouseNameOrNumber",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "HouseType",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "NumberOfOccupants",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "OtherHeatingType",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "OwnershipStatus",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "Postcode",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "PropertyType",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "RoofConstruction",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "RoofInsulated",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "SolidWallsInsulated",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "WallConstruction",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "YearBuilt",
                table: "PropertyData");

            migrationBuilder.RenameColumn(
                name: "PropertyDataId",
                table: "PropertyData",
                newName: "Id");
        }
    }
}
