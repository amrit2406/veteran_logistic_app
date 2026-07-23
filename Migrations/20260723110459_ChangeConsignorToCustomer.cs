using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace veteran_logistic.Migrations
{
    /// <inheritdoc />
    public partial class ChangeConsignorToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoadingRegisters_Companies_ConsignorId",
                table: "LoadingRegisters");

            migrationBuilder.AddForeignKey(
                name: "FK_LoadingRegisters_Customers_ConsignorId",
                table: "LoadingRegisters",
                column: "ConsignorId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoadingRegisters_Customers_ConsignorId",
                table: "LoadingRegisters");

            migrationBuilder.AddForeignKey(
                name: "FK_LoadingRegisters_Companies_ConsignorId",
                table: "LoadingRegisters",
                column: "ConsignorId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
