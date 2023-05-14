namespace Medfast.Services.MedicationAPI.Models.Dto.InventoryDto;

public class PharmacyMedicineCreateDto
{
    public string MedicineName { get; set; }
    public double Price { get; set; }
    public double? MedicineDiscount { get; set; }
    public int QuantityInStock { get; set; }
}