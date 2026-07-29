using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace veteran_logistic.Migrations
{
    /// <inheritdoc />
    public partial class AddPartyBillRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PartyBillRegisters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BillDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PartyId = table.Column<int>(type: "int", nullable: false),
                    ThirdPartyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PermitNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConsignorId = table.Column<int>(type: "int", nullable: true),
                    DestinationId = table.Column<int>(type: "int", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalRecords = table.Column<int>(type: "int", nullable: false),
                    TotalMaterialWeight = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ChargeHead1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChargeType1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChargeAmount1 = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ChargeHead2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChargeType2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChargeAmount2 = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
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
                    table.PrimaryKey("PK_PartyBillRegisters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyBillRegisters_Customers_ConsignorId",
                        column: x => x.ConsignorId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyBillRegisters_Customers_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyBillRegisters_SourceDestinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "SourceDestinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartyBillRegisterDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartyBillRegisterId = table.Column<int>(type: "int", nullable: false),
                    LoadingRegisterId = table.Column<int>(type: "int", nullable: false),
                    TPNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ChallanNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VehicleNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LoadingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaterialWeight = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    BillingRate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DriverCommission = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyBillRegisterDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyBillRegisterDetails_LoadingRegisters_LoadingRegisterId",
                        column: x => x.LoadingRegisterId,
                        principalTable: "LoadingRegisters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyBillRegisterDetails_PartyBillRegisters_PartyBillRegisterId",
                        column: x => x.PartyBillRegisterId,
                        principalTable: "PartyBillRegisters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartyBillRegisterDetails_LoadingRegisterId",
                table: "PartyBillRegisterDetails",
                column: "LoadingRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyBillRegisterDetails_PartyBillRegisterId",
                table: "PartyBillRegisterDetails",
                column: "PartyBillRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyBillRegisters_BillNumber",
                table: "PartyBillRegisters",
                column: "BillNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyBillRegisters_ConsignorId",
                table: "PartyBillRegisters",
                column: "ConsignorId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyBillRegisters_DestinationId",
                table: "PartyBillRegisters",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyBillRegisters_PartyId",
                table: "PartyBillRegisters",
                column: "PartyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartyBillRegisterDetails");

            migrationBuilder.DropTable(
                name: "PartyBillRegisters");
        }
    }
}
