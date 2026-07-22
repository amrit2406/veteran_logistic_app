using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace veteran_logistic.Migrations
{
    /// <inheritdoc />
    public partial class AddLoadingRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoadingRegisters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChallanNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConsignorId = table.Column<int>(type: "int", nullable: true),
                    ConsigneeId = table.Column<int>(type: "int", nullable: true),
                    SourceId = table.Column<int>(type: "int", nullable: true),
                    DestinationId = table.Column<int>(type: "int", nullable: true),
                    LoadingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TPNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: true),
                    VehicleType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UnionVendorId = table.Column<int>(type: "int", nullable: true),
                    DriverCommission = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    GrossWeight = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    TareWeight = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    LoadingWeight = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    VehicleLoadedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FuelQuantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    FuelAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FuelCash = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FuelAdvance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ShortageWeight = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    CashAdvance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentLocationId = table.Column<int>(type: "int", nullable: true),
                    OtherAdvance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OtherAdvanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThirdParty = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: true),
                    OwnerMobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OwnerAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Driver = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DrivingLicenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DriverMobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadingRegisters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadingRegisters_Companies_ConsignorId",
                        column: x => x.ConsignorId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadingRegisters_Customers_ConsigneeId",
                        column: x => x.ConsigneeId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadingRegisters_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadingRegisters_PaymentLocations_PaymentLocationId",
                        column: x => x.PaymentLocationId,
                        principalTable: "PaymentLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadingRegisters_SourceDestinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "SourceDestinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadingRegisters_SourceDestinations_SourceId",
                        column: x => x.SourceId,
                        principalTable: "SourceDestinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadingRegisters_VehicleOwners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "VehicleOwners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadingRegisters_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadingRegisters_Vendors_UnionVendorId",
                        column: x => x.UnionVendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoadingRegisters_ChallanNumber",
                table: "LoadingRegisters",
                column: "ChallanNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoadingRegisters_ConsigneeId",
                table: "LoadingRegisters",
                column: "ConsigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingRegisters_ConsignorId",
                table: "LoadingRegisters",
                column: "ConsignorId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingRegisters_DestinationId",
                table: "LoadingRegisters",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingRegisters_MaterialId",
                table: "LoadingRegisters",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingRegisters_OwnerId",
                table: "LoadingRegisters",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingRegisters_PaymentLocationId",
                table: "LoadingRegisters",
                column: "PaymentLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingRegisters_SourceId",
                table: "LoadingRegisters",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingRegisters_UnionVendorId",
                table: "LoadingRegisters",
                column: "UnionVendorId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingRegisters_VehicleId",
                table: "LoadingRegisters",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoadingRegisters");
        }
    }
}
