using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace veteran_logistic.Migrations
{
    /// <inheritdoc />
    public partial class ReorderVendorColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create a new table with columns in the correct order
            migrationBuilder.CreateTable(
                name: "Vendors_New",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CorrespondenceAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BillingAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ServiceTax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CST = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PAN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GSTIN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors_New", x => x.Id);
                });

            // Copy data from old table to new table
            migrationBuilder.Sql(@"
                INSERT INTO Vendors_New (Id, Code, Type, Name, CorrespondenceAddress, City, BillingAddress, Phone, Mobile, Fax, Email, ServiceTax, CST, PAN, GSTIN, IsActive, IsDeleted, DeletedOn, CreatedBy, ModifiedBy, CreatedOn, ModifiedOn)
                SELECT Id, Code, Type, Name, CorrespondenceAddress, City, BillingAddress, Phone, Mobile, Fax, Email, ServiceTax, CST, PAN, GSTIN, IsActive, IsDeleted, DeletedOn, CreatedBy, ModifiedBy, CreatedOn, ModifiedOn
                FROM Vendors
            ");

            // Drop old table
            migrationBuilder.DropTable(
                name: "Vendors");

            // Rename new table to original name
            migrationBuilder.RenameTable(
                name: "Vendors_New",
                newName: "Vendors");

            // Recreate unique index on Code
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

            // Create old table structure
            migrationBuilder.CreateTable(
                name: "Vendors_Old",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CorrespondenceAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BillingAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ServiceTax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CST = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PAN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GSTIN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors_Old", x => x.Id);
                });

            // Copy data back
            migrationBuilder.Sql(@"
                INSERT INTO Vendors_Old (Id, Code, Type, Name, CorrespondenceAddress, City, BillingAddress, Phone, Mobile, Fax, Email, ServiceTax, CST, PAN, GSTIN, IsActive, IsDeleted, DeletedOn, CreatedBy, ModifiedBy, CreatedOn, ModifiedOn)
                SELECT Id, Code, Type, Name, CorrespondenceAddress, City, BillingAddress, Phone, Mobile, Fax, Email, ServiceTax, CST, PAN, GSTIN, IsActive, IsDeleted, DeletedOn, CreatedBy, ModifiedBy, CreatedOn, ModifiedOn
                FROM Vendors
            ");

            // Drop current table
            migrationBuilder.DropTable(
                name: "Vendors");

            // Rename old table back
            migrationBuilder.RenameTable(
                name: "Vendors_Old",
                newName: "Vendors");

            // Recreate index
            migrationBuilder.CreateIndex(
                name: "IX_Vendors_Code",
                table: "Vendors",
                column: "Code",
                unique: true);
        }
    }
}
