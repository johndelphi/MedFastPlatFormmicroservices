using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medfast.Services.MedicationAPI.Migrations
{
    /// <inheritdoc />
    public partial class addcountycolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "Pharmacies",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "County",
                table: "Pharmacies");
        }
    }
}
