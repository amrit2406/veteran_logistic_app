using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace veteran_logistic.Migrations
{
    /// <inheritdoc />
    public partial class ReworkVendorEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vendors_GSTNumber",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_VendorCode",
                table: "Vendors");

            // Drop old columns that are no longer needed
            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "PANNumber",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "GSTNumber",
                table: "Vendors");

            // Rename VendorCode to Code
            migrationBuilder.RenameColumn(
                name: "VendorCode",
                table: "Vendors",
                newName: "Code");

            // Rename VendorName to Name
            migrationBuilder.RenameColumn(
                name: "VendorName",
                table: "Vendors",
                newName: "Name");

            // Rename PhoneNumber to Phone
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Vendors",
                newName: "Phone");

            // Add new columns
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CorrespondenceAddress",
                table: "Vendors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress",
                table: "Vendors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "Vendors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "Vendors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ServiceTax",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CST",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PAN",
                table: "Vendors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GSTIN",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            // Create unique index on Code
            migrationBuilder.CreateIndex(
                name: "IX_Vendors_Code",
                table: "Vendors",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vendors_Code",
                table: "Vendors");

            // Drop new columns
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "CorrespondenceAddress",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "BillingAddress",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ServiceTax",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "CST",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "PAN",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "GSTIN",
                table: "Vendors");

            // Rename Code back to VendorCode
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Vendors",
                newName: "VendorCode");

            // Rename Name back to VendorName
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Vendors",
                newName: "VendorName");

            // Rename Phone back to PhoneNumber
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Vendors",
                newName: "PhoneNumber");

            // Add back old columns
            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "Vendors",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Vendors",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "Vendors",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Vendors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PANNumber",
                table: "Vendors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GSTNumber",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            // Recreate old indexes
            migrationBuilder.CreateIndex(
                name: "IX_Vendors_GSTNumber",
                table: "Vendors",
                column: "GSTNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_VendorCode",
                table: "Vendors",
                column: "VendorCode",
                unique: true);
        }
    }
}
