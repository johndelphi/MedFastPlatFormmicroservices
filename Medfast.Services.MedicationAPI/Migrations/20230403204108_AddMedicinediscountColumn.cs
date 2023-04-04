using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medfast.Services.MedicationAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicinediscountColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MedicineDiscount",
                table: "Medicines",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            // Set random discounts for half of the medicines
            migrationBuilder.Sql("UPDATE Medicines SET MedicineDiscount = (SELECT ROUND((MAX(price) - RAND() * MAX(price)) / 2, 2) FROM Medicines) WHERE MedicineId % 2 = 0 AND price > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicineDiscount",
                table: "Medicines");
        }
    }
}
