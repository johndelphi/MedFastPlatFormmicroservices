using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Medfast.Services.MedicationAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace Medfast.Services.MedicationAPI.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Medicine?> Medicines { get; set; }
        public DbSet<Models.IdentityRole> IdentityRoles { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<PharmacyMedicine> PharmacyMedicines { get; set; }
        
        public DbSet<Transaction> Transactions { get; set; }
    }
}