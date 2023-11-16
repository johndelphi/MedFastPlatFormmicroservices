using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medfast.Services.MedicationAPI.Migrations
{
    /// <inheritdoc />
    public partial class Addingrolestotable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserPharmacyId",
                table: "Pharmacies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PharmacyId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PharmacyId",
                table: "AspNetUsers",
                column: "PharmacyId",
                unique: true,
                filter: "[PharmacyId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Pharmacies_PharmacyId",
                table: "AspNetUsers",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "PharmacyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Pharmacies_PharmacyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PharmacyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserPharmacyId",
                table: "Pharmacies");

            migrationBuilder.DropColumn(
                name: "PharmacyId",
                table: "AspNetUsers");
        }
    }
}
