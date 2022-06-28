using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    public partial class AddHasSeenAndHasEditedDataColumnsToPropertyData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasSeenRecommendations",
                table: "PropertyData",
                type: "boolean",
                nullable: true);
            migrationBuilder.AddColumn<bool>(
                name: "HasEditedData",
                table: "PropertyData",
                type: "boolean",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasSeenRecommendations",
                table: "PropertyData");
            migrationBuilder.DropColumn(
                name: "HasEditedData",
                table: "PropertyData");
        }
    }
}
