using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medfast.Services.MedicationAPI.Migrations
{
    /// <inheritdoc />
    public partial class newfieldonpharmacytable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Pharmacies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Pharmacies");
        }
    }
}
