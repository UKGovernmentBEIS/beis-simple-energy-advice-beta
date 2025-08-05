using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRerequestingRecommendationsFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RecommendationsLastRetrievedAt",
                table: "PropertyData",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RecommendationsUpdatedSinceLastVisit",
                table: "PropertyData",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecommendationsLastRetrievedAt",
                table: "PropertyData");

            migrationBuilder.DropColumn(
                name: "RecommendationsUpdatedSinceLastVisit",
                table: "PropertyData");
        }
    }
}
