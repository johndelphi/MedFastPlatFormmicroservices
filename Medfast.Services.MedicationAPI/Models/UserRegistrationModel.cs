using System.ComponentModel.DataAnnotations;

namespace Medfast.Services.MedicationAPI.Models
{
    public class UserRegistrationModel
    {
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
