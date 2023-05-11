using System.ComponentModel.DataAnnotations;
namespace Medfast.Services.MedicationAPI.Models.Dto.CreatePharmacyDto;

    public class PharmacyAccountCreateDto
    {
            [Required]
            [StringLength(50)]
            public string PharmacyName { get; set; }
    
            [Required]
            [StringLength(50)]
            public string Region { get; set; }
    
            [Required]
            [StringLength(20)]
            public string PhoneNumber { get; set; }
    
            [Required]
            public double DistanceInKm { get; set; }
    
            [Required]
            [StringLength(50)]
            public string City { get; set; }
    
            [Required]
            [StringLength(50)]
            public string SubCity { get; set; }
    
            [Required]
            [StringLength(50)]
            public string Landmark { get; set; }
    
            [Required]
            public double Latitude { get; set; }
    
            [Required]
            public double Longitude { get; set; }
    
            [Required]
            [StringLength(50)]
            public string UserName { get; set; }
    
            [Required]
            [StringLength(50)]
            [EmailAddress]
            public string Email { get; set; }
    
            [Required]
            [StringLength(20, MinimumLength = 6)]
            public string Password { get; set; }
    }
           