using System.ComponentModel.DataAnnotations;
using Medfast.Services.MedicationAPI.Models.Dto;

namespace Medfast.Services.MedicationAPI.Models;

public class PharmacyMedicine
{
    [Key]
    public int PharmacyMedicineId { get; set; }

    public int MedicineId { get; set; }
    public Medicine Medicine { get; set; }
   
    public int PharmacyId { get; set; }
    public Pharmacy Pharmacy { get; set; }

    [Required]
    public double Price { get; set; }

    public double? MedicineDiscount { get; set; }

    [Required]
    public int QuantityInStock { get; set; }

    // Navigation property
    public List<Transaction> Transactions { get; set; }
}
