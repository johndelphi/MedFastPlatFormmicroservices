using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medfast.Services.MedicationAPI.Migrations
{
    public partial class correctrelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Pharmacies_PharmacyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_PharmacyMedicines_PharmacyMedicineId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "IdentityRoles");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_PharmacyMedicineId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PharmacyMedicines",
                table: "PharmacyMedicines");

            migrationBuilder.DropIndex(
                name: "IX_PharmacyMedicines_PharmacyId",
                table: "PharmacyMedicines");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PharmacyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserPharmacyId",
                table: "Pharmacies");

            // Remove AspNetUsers records with non-existent PharmacyId
            migrationBuilder.Sql(
                "DELETE FROM AspNetUsers WHERE PharmacyId NOT IN (SELECT PharmacyId FROM Pharmacies)");

            migrationBuilder.AddColumn<int>(
                name: "PharmacyMedicineMedicineId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PharmacyMedicinePharmacyId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DosageForm",
                table: "Medicines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "Medicines",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ManufactureDate",
                table: "Medicines",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Medicines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Medicines",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Strength",
                table: "Medicines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // Recreate the primary key constraint with the new columns
            migrationBuilder.AddPrimaryKey(
                name: "PK_PharmacyMedicines",
                table: "PharmacyMedicines",
                columns: new[] { "PharmacyId", "MedicineId" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PharmacyMedicinePharmacyId_PharmacyMedicineMedicineId",
                table: "Transactions",
                columns: new[] { "PharmacyMedicinePharmacyId", "PharmacyMedicineMedicineId" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PharmacyId",
                table: "AspNetUsers",
                column: "PharmacyId");

            // Recreate foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Pharmacies_PharmacyId",
                table: "AspNetUsers",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "PharmacyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Pharmacies_PharmacyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_PharmacyMedicinePharmacyId_PharmacyMedicineMedicineId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PharmacyMedicines",
                table: "PharmacyMedicines");

            migrationBuilder.DropColumn(
                name: "PharmacyMedicineMedicineId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PharmacyMedicinePharmacyId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "DosageForm",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "ManufactureDate",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "Strength",
                table: "Medicines");

            // Recreate original table and constraint structures
            // This includes adding back any dropped columns, re-creating dropped indexes and foreign keys
            // The exact statements depend on the original schema before the Up() migration was applied

            // Add back the IdentityRoles table
            migrationBuilder.CreateTable(
                name: "IdentityRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRoles", x => x.Id);
                });

            // Other reverse logic as needed
        }
    }
}
