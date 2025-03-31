using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    public partial class AddHeatingControlsField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "HeatingControls",
                table: "PropertyData",
                type: "integer[]",
                defaultValueSql: "ARRAY[]::integer[]",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeatingControls",
                table: "PropertyData");
        }
    }
}
