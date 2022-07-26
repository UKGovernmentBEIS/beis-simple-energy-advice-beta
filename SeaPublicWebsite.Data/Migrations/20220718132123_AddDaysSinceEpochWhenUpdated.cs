using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    public partial class AddDaysSinceEpochWhenUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdatedTicks",
                table: "PropertyData",
                newName: "DaysSinceEpochWhenUpdated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DaysSinceEpochWhenUpdated",
                table: "PropertyData",
                newName: "LastUpdatedTicks");
        }
    }
}
