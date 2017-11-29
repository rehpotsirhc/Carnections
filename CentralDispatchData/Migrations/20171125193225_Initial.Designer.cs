﻿// <auto-generated />
using CentralDispatchData;
using Enums.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CentralDispatchData.Migrations
{
    [DbContext(typeof(CDListingDbContext))]
    [Migration("20171125193225_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Common.Models.LonLat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.HasKey("Id");

                    b.ToTable("LonLat");
                });

            modelBuilder.Entity("Common.Models.TrailerTypeWeight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Type");

                    b.Property<double>("Weight");

                    b.HasKey("Id");

                    b.ToTable("TrailerTypeWeight");
                });

            modelBuilder.Entity("Common.Models.TransformedListing", b =>
                {
                    b.Property<int>("ListingId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("AverageVehicleWeight");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DeliveryId");

                    b.Property<bool>("LocationsValid");

                    b.Property<int>("MilesInterpolated");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int?>("PickupId");

                    b.Property<double>("Price");

                    b.Property<double>("PricePerMile");

                    b.Property<int>("ShipMethod");

                    b.Property<int?>("TrailerTypeWeightId");

                    b.Property<string>("TruckMiles");

                    b.Property<int>("VehicleCount");

                    b.Property<bool>("VehicleOperable");

                    b.Property<string>("VehicleTypes");

                    b.HasKey("ListingId");

                    b.HasIndex("DeliveryId");

                    b.HasIndex("PickupId");

                    b.HasIndex("TrailerTypeWeightId");

                    b.ToTable("TransformedListing");
                });

            modelBuilder.Entity("Common.Models.VehicleSizeWeight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Size");

                    b.Property<double>("Weight");

                    b.HasKey("Id");

                    b.ToTable("VehicleSizeWeight");
                });

            modelBuilder.Entity("Common.Models.VehicleTypeSize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("SizeWeightId");

                    b.Property<int?>("TransformedListingListingId");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("SizeWeightId");

                    b.HasIndex("TransformedListingListingId");

                    b.ToTable("VehicleTypeSize");
                });

            modelBuilder.Entity("Common.Models.TransformedListing", b =>
                {
                    b.HasOne("Common.Models.LonLat", "Delivery")
                        .WithMany()
                        .HasForeignKey("DeliveryId");

                    b.HasOne("Common.Models.LonLat", "Pickup")
                        .WithMany()
                        .HasForeignKey("PickupId");

                    b.HasOne("Common.Models.TrailerTypeWeight", "TrailerTypeWeight")
                        .WithMany()
                        .HasForeignKey("TrailerTypeWeightId");
                });

            modelBuilder.Entity("Common.Models.VehicleTypeSize", b =>
                {
                    b.HasOne("Common.Models.VehicleSizeWeight", "SizeWeight")
                        .WithMany()
                        .HasForeignKey("SizeWeightId");

                    b.HasOne("Common.Models.TransformedListing")
                        .WithMany("VehicleTypesSizes")
                        .HasForeignKey("TransformedListingListingId");
                });
#pragma warning restore 612, 618
        }
    }
}
