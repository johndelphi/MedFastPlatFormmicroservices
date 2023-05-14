using System.ComponentModel.DataAnnotations;
namespace Medfast.Services.MedicationAPI.Models.Dto.CreatePharmacyDto;

    public class PharmacyAccountCreateDto
    {
             [Required(ErrorMessage = "Pharmacy name is required.")]
        [MaxLength(100, ErrorMessage = "Pharmacy name cannot exceed 100 characters.")]
        public string PharmacyName { get; set; }

        [MaxLength(50, ErrorMessage = "Region cannot exceed 50 characters.")]
        public string Region { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string City { get; set; }

        [MaxLength(50, ErrorMessage = "Sub city cannot exceed 50 characters.")]
        public string SubCity { get; set; }

        [MaxLength(100, ErrorMessage = "Landmark cannot exceed 100 characters.")]
        public string Landmark { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Latitude is required.")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required.")]
        public double Longitude { get; set; }
    }
           