using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace veteran_logistic.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDORatesToUseIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Consignee",
                table: "DORates");

            migrationBuilder.DropColumn(
                name: "Consignor",
                table: "DORates");

            migrationBuilder.AddColumn<int>(
                name: "ConsigneeId",
                table: "DORates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConsignorId",
                table: "DORates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsigneeId",
                table: "DORates");

            migrationBuilder.DropColumn(
                name: "ConsignorId",
                table: "DORates");

            migrationBuilder.AddColumn<string>(
                name: "Consignee",
                table: "DORates",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Consignor",
                table: "DORates",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
