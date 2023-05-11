
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

        public string Category { get; set; }

        public string ImageUrl { get; set; }

        // Navigation property
        public List<PharmacyMedicine> PharmacyMedicines { get; set; }
    }

   
}
