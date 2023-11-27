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

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<PharmacyMedicine> PharmacyMedicines { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-many relationship between Pharmacy and ApplicationUser
            modelBuilder.Entity<Pharmacy>()
                .HasMany(p => p.ApplicationUsers)
                .WithOne(u => u.Pharmacy)
                .HasForeignKey(u => u.PharmacyId);

            // Configure the many-to-many relationship between Pharmacy and Medicine
            modelBuilder.Entity<PharmacyMedicine>()
                .HasKey(pm => new { pm.PharmacyId, pm.MedicineId });

            modelBuilder.Entity<PharmacyMedicine>()
                .HasOne(pm => pm.Pharmacy)
                .WithMany(p => p.PharmacyMedicines)
                .HasForeignKey(pm => pm.PharmacyId);

            modelBuilder.Entity<PharmacyMedicine>()
                .HasOne(pm => pm.Medicine)
                .WithMany(m => m.PharmacyMedicines)
                .HasForeignKey(pm => pm.MedicineId);

            // Additional entity configurations can be added here
        }
    }
}
