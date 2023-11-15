using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medfast.Services.MedicationAPI.Models
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        [MaxLength(50)]
        public override string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public override string Email { get; set; }

        public string Role { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [ForeignKey("Pharmacy")]
        public int PharmacyId { get; set; }

        public virtual Pharmacy Pharmacy { get; set; }

        // Navigation property
        public List<Transaction> Transactions { get; set; }
    }
}
