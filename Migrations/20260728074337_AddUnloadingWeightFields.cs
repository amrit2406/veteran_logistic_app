using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace veteran_logistic.Migrations
{
    /// <inheritdoc />
    public partial class AddUnloadingWeightFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ChallanMoney",
                table: "UnloadingRegisters",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GrossWeightUL",
                table: "UnloadingRegisters",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TareWeightUL",
                table: "UnloadingRegisters",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnloadingWeight",
                table: "UnloadingRegisters",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChallanMoney",
                table: "UnloadingRegisters");

            migrationBuilder.DropColumn(
                name: "GrossWeightUL",
                table: "UnloadingRegisters");

            migrationBuilder.DropColumn(
                name: "TareWeightUL",
                table: "UnloadingRegisters");

            migrationBuilder.DropColumn(
                name: "UnloadingWeight",
                table: "UnloadingRegisters");
        }
    }
}
