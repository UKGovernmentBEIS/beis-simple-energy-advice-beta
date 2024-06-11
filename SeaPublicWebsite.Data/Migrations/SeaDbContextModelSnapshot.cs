﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SeaPublicWebsite.Data;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    [DbContext(typeof(SeaDbContext))]
    partial class SeaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.DataProtection.EntityFrameworkCore.DataProtectionKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FriendlyName")
                        .HasColumnType("text");

                    b.Property<string>("Xml")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DataProtectionKeys");
                });

            modelBuilder.Entity("SeaPublicWebsite.BusinessLogic.Models.Epc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("BungalowType")
                        .HasColumnType("integer");

                    b.Property<int?>("CavityWallsInsulated")
                        .HasColumnType("integer");

                    b.Property<int?>("ConstructionAgeBand")
                        .HasColumnType("integer");

                    b.Property<int?>("EpcHeatingType")
                        .HasColumnType("integer");

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

                    b.Property<int?>("HouseType")
                        .HasColumnType("integer");

                    b.Property<int?>("LodgementYear")
                        .HasColumnType("integer");

                    b.Property<int>("PropertyDataId")
                        .HasColumnType("integer");

                    b.Property<int?>("PropertyType")
                        .HasColumnType("integer");

                    b.Property<int?>("RoofConstruction")
                        .HasColumnType("integer");

                    b.Property<int?>("RoofInsulated")
                        .HasColumnType("integer");

                    b.Property<int?>("SolidWallsInsulated")
                        .HasColumnType("integer");

                    b.Property<int?>("WallConstruction")
                        .HasColumnType("integer");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id");

                    b.HasIndex("PropertyDataId")
                        .IsUnique();

                    b.ToTable("Epc");
                });

            modelBuilder.Entity("SeaPublicWebsite.BusinessLogic.Models.PropertyData", b =>
                {
                    b.Property<int>("PropertyDataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PropertyDataId"));

                    b.Property<int?>("BungalowType")
                        .HasColumnType("integer");

                    b.Property<int?>("CavityWallsInsulated")
                        .HasColumnType("integer");

                    b.Property<int?>("Country")
                        .HasColumnType("integer");

                    b.Property<int?>("EditedDataId")
                        .HasColumnType("integer");

                    b.Property<int?>("EpcDetailsConfirmed")
                        .HasColumnType("integer");

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

                    b.Property<bool>("HasSeenRecommendations")
                        .HasColumnType("boolean");

                    b.Property<int?>("HeatingPattern")
                        .HasColumnType("integer");

                    b.Property<int?>("HeatingType")
                        .HasColumnType("integer");

                    b.Property<int?>("HoursOfHeatingEvening")
                        .HasColumnType("integer");

                    b.Property<int?>("HoursOfHeatingMorning")
                        .HasColumnType("integer");

                    b.Property<int?>("HouseType")
                        .HasColumnType("integer");

                    b.Property<int?>("LoftAccess")
                        .HasColumnType("integer");

                    b.Property<int?>("LoftSpace")
                        .HasColumnType("integer");

                    b.Property<int?>("NumberOfOccupants")
                        .HasColumnType("integer");

                    b.Property<int?>("OtherHeatingType")
                        .HasColumnType("integer");

                    b.Property<int?>("OwnershipStatus")
                        .HasColumnType("integer");

                    b.Property<int?>("PropertyType")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("RecommendationsFirstRetrievedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Reference")
                        .HasColumnType("text");

                    b.Property<bool>("ReturningUser")
                        .HasColumnType("boolean");

                    b.Property<int?>("RoofConstruction")
                        .HasColumnType("integer");

                    b.Property<int?>("RoofInsulated")
                        .HasColumnType("integer");

                    b.Property<int?>("SearchForEpc")
                        .HasColumnType("integer");

                    b.Property<int?>("SolidWallsInsulated")
                        .HasColumnType("integer");

                    b.Property<decimal?>("Temperature")
                        .HasColumnType("numeric");

                    b.Property<int?>("WallConstruction")
                        .HasColumnType("integer");

                    b.Property<int?>("YearBuilt")
                        .HasColumnType("integer");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("PropertyDataId");

                    b.HasIndex("EditedDataId")
                        .IsUnique();

                    b.HasIndex("Reference")
                        .IsUnique();

                    b.ToTable("PropertyData");
                });

            modelBuilder.Entity("SeaPublicWebsite.BusinessLogic.Models.PropertyRecommendation", b =>
                {
                    b.Property<int>("PropertyRecommendationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PropertyRecommendationId"));

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

                    b.Property<int>("PropertyDataId")
                        .HasColumnType("integer");

                    b.Property<int?>("RecommendationAction")
                        .HasColumnType("integer");

                    b.Property<int>("Saving")
                        .HasColumnType("integer");

                    b.Property<string>("Summary")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("PropertyRecommendationId");

                    b.HasIndex("PropertyDataId");

                    b.ToTable("PropertyRecommendations");
                });

            modelBuilder.Entity("SeaPublicWebsite.BusinessLogic.Models.Epc", b =>
                {
                    b.HasOne("SeaPublicWebsite.BusinessLogic.Models.PropertyData", null)
                        .WithOne("Epc")
                        .HasForeignKey("SeaPublicWebsite.BusinessLogic.Models.Epc", "PropertyDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SeaPublicWebsite.BusinessLogic.Models.PropertyData", b =>
                {
                    b.HasOne("SeaPublicWebsite.BusinessLogic.Models.PropertyData", null)
                        .WithOne("UneditedData")
                        .HasForeignKey("SeaPublicWebsite.BusinessLogic.Models.PropertyData", "EditedDataId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SeaPublicWebsite.BusinessLogic.Models.PropertyRecommendation", b =>
                {
                    b.HasOne("SeaPublicWebsite.BusinessLogic.Models.PropertyData", "PropertyData")
                        .WithMany("PropertyRecommendations")
                        .HasForeignKey("PropertyDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PropertyData");
                });

            modelBuilder.Entity("SeaPublicWebsite.BusinessLogic.Models.PropertyData", b =>
                {
                    b.Navigation("Epc");

                    b.Navigation("PropertyRecommendations");

                    b.Navigation("UneditedData");
                });
#pragma warning restore 612, 618
        }
    }
}
