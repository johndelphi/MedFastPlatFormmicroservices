using System.ComponentModel.DataAnnotations;


namespace Medfast.Services.MedicationAPI.Models;
public class Pharmacy
{
    [Key]
    public int PharmacyId { get; set; }

    [Required]
    [MaxLength(100)]
    public string PharmacyName { get; set; }

    [MaxLength(50)]
    public string Region { get; set; }

    [MaxLength(50)]
    public string City { get; set; }

    [MaxLength(50)]
    public string SubCity { get; set; }

    [MaxLength(100)]
    public string Landmark { get; set; }
    
    
    [Phone]
    public string PhoneNumber { get; set; }
    
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    // Updated navigation property
    public ICollection<ApplicationUser> ApplicationUsers { get; set; }

    // Navigation property
    public List<PharmacyMedicine> PharmacyMedicines { get; set; }
}
