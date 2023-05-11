using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medfast.Services.MedicationAPI.Migrations
{
    /// <inheritdoc />
    public partial class seedtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
    // Seed data for User table
    
    migrationBuilder.Sql("INSERT INTO Users(UserId, Username, Email, Role) VALUES (NEWID(), 'User1', 'user1@example.com', 'User')");
    migrationBuilder.Sql("INSERT INTO Users(UserId, Username, Email, Role) VALUES (NEWID(), 'User2', 'user2@example.com', 'User')");
    
    // Seed data for Pharmacy table
    migrationBuilder.Sql("SET IDENTITY_INSERT Pharmacies ON");
    migrationBuilder.Sql("INSERT INTO Pharmacies(PharmacyId, PharmacyName, Region, City, SubCity, Landmark) VALUES (1, 'Pharmacy1', 'Region1', 'City1', 'SubCity1', 'Landmark1')");
    migrationBuilder.Sql("INSERT INTO Pharmacies(PharmacyId, PharmacyName, Region, City, SubCity, Landmark) VALUES (2, 'Pharmacy2', 'Region2', 'City2', 'SubCity2', 'Landmark2')");
// Disable IDENTITY_INSERT
    migrationBuilder.Sql("SET IDENTITY_INSERT Pharmacies OFF");
    // Seed data for Medicine table
    migrationBuilder.Sql("SET IDENTITY_INSERT Medicines ON");
    migrationBuilder.Sql("INSERT INTO Medicines(MedicineId, MedicineName, MedicineDescription, Category, ImageUrl) VALUES (1, 'Medicine1', 'Description1', 'Category1', 'ImageUrl1')");
    migrationBuilder.Sql("INSERT INTO Medicines(MedicineId, MedicineName, MedicineDescription, Category, ImageUrl) VALUES (2, 'Medicine2', 'Description2', 'Category2', 'ImageUrl2')");
    migrationBuilder.Sql("SET IDENTITY_INSERT Medicines OFF");
    // Seed data for PharmacyMedicine table
    migrationBuilder.Sql("SET IDENTITY_INSERT PharmacyMedicines ON");
    migrationBuilder.Sql("INSERT INTO PharmacyMedicines(PharmacyMedicineId, MedicineId, PharmacyId, Price, MedicineDiscount, QuantityInStock) VALUES (1, 1, 1, 100, 10, 50)");
    migrationBuilder.Sql("INSERT INTO PharmacyMedicines(PharmacyMedicineId, MedicineId, PharmacyId, Price, MedicineDiscount, QuantityInStock) VALUES (2, 2, 2, 200, 20, 100)");
    migrationBuilder.Sql("SET IDENTITY_INSERT PharmacyMedicines OFF");
    // Seed data for Transaction table
   
    migrationBuilder.Sql("INSERT INTO Transactions(TransactionId, QuantityPurchased, PurchasePrice, PurchaseDate, UserId, PharmacyMedicineId) VALUES (NEWID(), 2, 100, GETDATE(), (SELECT UserId FROM Users WHERE Username = 'User1'), 1)");
    migrationBuilder.Sql("INSERT INTO Transactions(TransactionId, QuantityPurchased, PurchasePrice, PurchaseDate, UserId, PharmacyMedicineId) VALUES (NEWID(), 3, 200, GETDATE(), (SELECT UserId FROM Users WHERE Username = 'User2'), 2)");
    
    
}
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
