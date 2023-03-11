

using Medfast.Services.MedicationAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Medfast.Services.MedicationAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options): base(options)
        {


        }
        public  DbSet<Medicine>Medicines { get; set; }
    }
}
