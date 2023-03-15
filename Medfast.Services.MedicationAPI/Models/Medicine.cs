
using System.ComponentModel.DataAnnotations;

namespace Medfast.Services.MedicationAPI.Models
{
    public class Medicine
    {
        [Key]
        public  int MedicineId { get; set; }

        [Required]
        public string MedicineName { get; set; }

        [Required]
        public string MedicineDescription { get;set; }
        [Required]
        public  double price { get; set; }

        public string Category { get; set; }

        public  string ImageUrl { get; set; }

    public static implicit operator Medicine(List<Medicine> v)
    {
      throw new NotImplementedException();
    }
  }
}
