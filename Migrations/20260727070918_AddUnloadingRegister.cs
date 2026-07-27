using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace veteran_logistic.Migrations
{
    /// <inheritdoc />
    public partial class AddUnloadingRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnloadingRegisters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChallanNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConsignorId = table.Column<int>(type: "int", nullable: true),
                    ConsigneeId = table.Column<int>(type: "int", nullable: true),
                    SourceId = table.Column<int>(type: "int", nullable: true),
                    DestinationId = table.Column<int>(type: "int", nullable: true),
                    UnloadingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_UnloadingRegisters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnloadingRegisters_Customers_ConsigneeId",
                        column: x => x.ConsigneeId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnloadingRegisters_Customers_ConsignorId",
                        column: x => x.ConsignorId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnloadingRegisters_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnloadingRegisters_PaymentLocations_PaymentLocationId",
                        column: x => x.PaymentLocationId,
                        principalTable: "PaymentLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnloadingRegisters_SourceDestinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "SourceDestinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnloadingRegisters_SourceDestinations_SourceId",
                        column: x => x.SourceId,
                        principalTable: "SourceDestinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnloadingRegisters_VehicleOwners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "VehicleOwners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnloadingRegisters_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnloadingRegisters_Vendors_UnionVendorId",
                        column: x => x.UnionVendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnloadingRegisters_ChallanNumber",
                table: "UnloadingRegisters",
                column: "ChallanNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnloadingRegisters_ConsigneeId",
                table: "UnloadingRegisters",
                column: "ConsigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_UnloadingRegisters_ConsignorId",
                table: "UnloadingRegisters",
                column: "ConsignorId");

            migrationBuilder.CreateIndex(
                name: "IX_UnloadingRegisters_DestinationId",
                table: "UnloadingRegisters",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_UnloadingRegisters_MaterialId",
                table: "UnloadingRegisters",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_UnloadingRegisters_OwnerId",
                table: "UnloadingRegisters",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UnloadingRegisters_PaymentLocationId",
                table: "UnloadingRegisters",
                column: "PaymentLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_UnloadingRegisters_SourceId",
                table: "UnloadingRegisters",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_UnloadingRegisters_UnionVendorId",
                table: "UnloadingRegisters",
                column: "UnionVendorId");

            migrationBuilder.CreateIndex(
                name: "IX_UnloadingRegisters_VehicleId",
                table: "UnloadingRegisters",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnloadingRegisters");
        }
    }
}
