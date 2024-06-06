using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace Medfast.Services.MedicationAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public string PasswordResetCode { get; set; }
        public DateTime? ResetTokenExpiration { get; set; }
    }
}

