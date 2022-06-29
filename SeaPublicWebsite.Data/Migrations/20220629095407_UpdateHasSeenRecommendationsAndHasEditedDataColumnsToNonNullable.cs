using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    public partial class UpdateHasSeenRecommendationsAndHasEditedDataColumnsToNonNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasEditedData",
                table: "PropertyData");

            migrationBuilder.AlterColumn<bool>(
                name: "HasSeenRecommendations",
                table: "PropertyData",
                defaultValue: false,
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasEditedData",
                table: "PropertyData",
                type: "boolean",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasSeenRecommendations",
                table: "PropertyData",
                nullable: true);
        }
    }
}
