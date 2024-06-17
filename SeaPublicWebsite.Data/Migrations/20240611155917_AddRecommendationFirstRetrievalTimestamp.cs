using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    public partial class AddRecommendationFirstRetrievalTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RecommendationsFirstRetrievedAt",
                table: "PropertyData",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecommendationsFirstRetrievedAt",
                table: "PropertyData");
        }
    }
}
