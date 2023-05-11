using System.ComponentModel.DataAnnotations;

namespace Medfast.Services.MedicationAPI.Models;

public class User
{
    [Key]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    [MaxLength(50)]
    public string Email { get; set; }

    public string Role { get; set; }

    // Navigation property
    public List<Transaction> Transactions { get; set; }
}