namespace Medfast.Services.MedicationAPI.Models.Dto;

public class TransactionDto
{
    public Guid TransactionId { get; set; }
    public int QuantityPurchased { get; set; }
    public double PurchasePrice { get; set; }
    public DateTime PurchaseDate { get; set; }
    public Guid UserId { get; set; }
    public int PharmacyMedicineId { get; set; }
}