
using System.ComponentModel.DataAnnotations;

namespace Medfast.Services.MedicationAPI.Models
{
    public class Medicine
    {
        [Key]
        public int MedicineId { get; set; }

        [Required]
        public string MedicineName { get; set; }

        [MaxLength(500)]
        public string MedicineDescription { get; set; }

        public string DosageForm { get; set; }

        public string Strength { get; set; }

        public string Manufacturer { get; set; }

        public string Category { get; set; }

        public string ImageUrl { get; set; }

        public double Price { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Barcode { get; set; }

        // Navigation property
        public List<PharmacyMedicine> PharmacyMedicines { get; set; }
    }

   
}
