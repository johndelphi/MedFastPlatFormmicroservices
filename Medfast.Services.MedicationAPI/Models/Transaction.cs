using System.ComponentModel.DataAnnotations;

namespace Medfast.Services.MedicationAPI.Models;

public class Transaction
{
    [Key]
    public Guid TransactionId { get; set; }

    [Required]
    public int QuantityPurchased { get; set; }

    [Required]
    public double PurchasePrice { get; set; }

    [Required]
    public DateTime PurchaseDate { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } // Navigation property

    public int PharmacyMedicineId { get; set; }
    public PharmacyMedicine PharmacyMedicine { get; set; } // Navigation property
}