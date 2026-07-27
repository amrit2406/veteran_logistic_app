using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace veteran_logistic.Migrations
{
    /// <inheritdoc />
    public partial class AddLoadingRegisterIdToUnloadingRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoadingRegisterId",
                table: "UnloadingRegisters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnloadingRegisters_LoadingRegisterId",
                table: "UnloadingRegisters",
                column: "LoadingRegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_UnloadingRegisters_LoadingRegisters_LoadingRegisterId",
                table: "UnloadingRegisters",
                column: "LoadingRegisterId",
                principalTable: "LoadingRegisters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnloadingRegisters_LoadingRegisters_LoadingRegisterId",
                table: "UnloadingRegisters");

            migrationBuilder.DropIndex(
                name: "IX_UnloadingRegisters_LoadingRegisterId",
                table: "UnloadingRegisters");

            migrationBuilder.DropColumn(
                name: "LoadingRegisterId",
                table: "UnloadingRegisters");
        }
    }
}
