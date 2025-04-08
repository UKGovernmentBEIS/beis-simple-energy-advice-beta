using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHeatingControlsEPCField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "HeatingControls",
                table: "Epc",
                type: "integer[]",
                defaultValueSql: "ARRAY[]::integer[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeatingControls",
                table: "Epc");
        }
    }
}