using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CentralDispatchData.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LonLat",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LonLat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrailerTypeWeight",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false),
                    Weight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrailerTypeWeight", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleSizeWeight",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Size = table.Column<int>(nullable: false),
                    Weight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleSizeWeight", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransformedListing",
                columns: table => new
                {
                    ListingId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AverageVehicleWeight = table.Column<double>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeliveryId = table.Column<int>(nullable: true),
                    LocationsValid = table.Column<bool>(nullable: false),
                    MilesInterpolated = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    PickupId = table.Column<int>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    PricePerMile = table.Column<double>(nullable: false),
                    ShipMethod = table.Column<int>(nullable: false),
                    TrailerTypeWeightId = table.Column<int>(nullable: true),
                    TruckMiles = table.Column<string>(nullable: true),
                    VehicleCount = table.Column<int>(nullable: false),
                    VehicleOperable = table.Column<bool>(nullable: false),
                    VehicleTypes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransformedListing", x => x.ListingId);
                    table.ForeignKey(
                        name: "FK_TransformedListing_LonLat_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "LonLat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransformedListing_LonLat_PickupId",
                        column: x => x.PickupId,
                        principalTable: "LonLat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransformedListing_TrailerTypeWeight_TrailerTypeWeightId",
                        column: x => x.TrailerTypeWeightId,
                        principalTable: "TrailerTypeWeight",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypeSize",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SizeWeightId = table.Column<int>(nullable: true),
                    TransformedListingListingId = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypeSize", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleTypeSize_VehicleSizeWeight_SizeWeightId",
                        column: x => x.SizeWeightId,
                        principalTable: "VehicleSizeWeight",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleTypeSize_TransformedListing_TransformedListingListingId",
                        column: x => x.TransformedListingListingId,
                        principalTable: "TransformedListing",
                        principalColumn: "ListingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransformedListing_DeliveryId",
                table: "TransformedListing",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_TransformedListing_PickupId",
                table: "TransformedListing",
                column: "PickupId");

            migrationBuilder.CreateIndex(
                name: "IX_TransformedListing_TrailerTypeWeightId",
                table: "TransformedListing",
                column: "TrailerTypeWeightId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypeSize_SizeWeightId",
                table: "VehicleTypeSize",
                column: "SizeWeightId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypeSize_TransformedListingListingId",
                table: "VehicleTypeSize",
                column: "TransformedListingListingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleTypeSize");

            migrationBuilder.DropTable(
                name: "VehicleSizeWeight");

            migrationBuilder.DropTable(
                name: "TransformedListing");

            migrationBuilder.DropTable(
                name: "LonLat");

            migrationBuilder.DropTable(
                name: "TrailerTypeWeight");
        }
    }
}
