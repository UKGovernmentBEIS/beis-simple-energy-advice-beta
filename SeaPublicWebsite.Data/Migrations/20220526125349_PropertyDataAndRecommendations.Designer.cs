﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SeaPublicWebsite.Data;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    [DbContext(typeof(SeaDbContext))]
    [Migration("20220526125349_PropertyDataAndRecommendations")]
    partial class PropertyDataAndRecommendations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SeaPublicWebsite.Data.PropertyData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AccessibleLoftSpace")
                        .HasColumnType("integer");

                    b.Property<int?>("BungalowType")
                        .HasColumnType("integer");

                    b.Property<int?>("CavityWallsInsulated")
                        .HasColumnType("integer");

                    b.Property<int?>("Country")
                        .HasColumnType("integer");

                    b.Property<string>("EpcLmkKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("FlatType")
                        .HasColumnType("integer");

                    b.Property<int?>("FloorConstruction")
                        .HasColumnType("integer");

                    b.Property<int?>("FloorInsulated")
                        .HasColumnType("integer");

                    b.Property<int?>("GlazingType")
                        .HasColumnType("integer");

                    b.Property<int?>("HasHotWaterCylinder")
                        .HasColumnType("integer");

                    b.Property<int?>("HasOutdoorSpace")
                        .HasColumnType("integer");

                    b.Property<int?>("HeatingPattern")
                        .HasColumnType("integer");

                    b.Property<int?>("HeatingType")
                        .HasColumnType("integer");

                    b.Property<decimal?>("HoursOfHeating")
                        .HasColumnType("numeric");

                    b.Property<string>("HouseNameOrNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("HouseType")
                        .HasColumnType("integer");

                    b.Property<int?>("NumberOfOccupants")
                        .HasColumnType("integer");

                    b.Property<int?>("OtherHeatingType")
                        .HasColumnType("integer");

                    b.Property<int?>("OwnershipStatus")
                        .HasColumnType("integer");

                    b.Property<string>("Postcode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("PropertyType")
                        .HasColumnType("integer");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("RoofConstruction")
                        .HasColumnType("integer");

                    b.Property<int?>("RoofInsulated")
                        .HasColumnType("integer");

                    b.Property<int?>("SolidWallsInsulated")
                        .HasColumnType("integer");

                    b.Property<decimal?>("Temperature")
                        .HasColumnType("numeric");

                    b.Property<int?>("WallConstruction")
                        .HasColumnType("integer");

                    b.Property<int?>("YearBuilt")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("PropertyData");
                });

            modelBuilder.Entity("SeaPublicWebsite.Data.PropertyRecommendation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Key")
                        .HasColumnType("integer");

                    b.Property<int>("Lifetime")
                        .HasColumnType("integer");

                    b.Property<int>("LifetimeSaving")
                        .HasColumnType("integer");

                    b.Property<int>("MaxInstallCost")
                        .HasColumnType("integer");

                    b.Property<int>("MinInstallCost")
                        .HasColumnType("integer");

                    b.Property<int?>("PropertyDataId")
                        .HasColumnType("integer");

                    b.Property<int?>("RecommendationAction")
                        .HasColumnType("integer");

                    b.Property<int>("Saving")
                        .HasColumnType("integer");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PropertyDataId");

                    b.ToTable("PropertyRecommendations");
                });

            modelBuilder.Entity("SeaPublicWebsite.Data.PropertyRecommendation", b =>
                {
                    b.HasOne("SeaPublicWebsite.Data.PropertyData", null)
                        .WithMany("PropertyRecommendations")
                        .HasForeignKey("PropertyDataId");
                });

            modelBuilder.Entity("SeaPublicWebsite.Data.PropertyData", b =>
                {
                    b.Navigation("PropertyRecommendations");
                });
#pragma warning restore 612, 618
        }
    }
}
