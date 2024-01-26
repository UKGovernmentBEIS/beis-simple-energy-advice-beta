using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    public partial class AddIsLatestAssessmentForAddressToEpc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLatestAssessmentForAddress",
                table: "Epc",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLatestAssessmentForAddress",
                table: "Epc");
        }
    }
}
