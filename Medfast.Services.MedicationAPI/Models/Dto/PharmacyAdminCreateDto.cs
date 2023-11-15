namespace Medfast.Services.MedicationAPI.Models.Dto
{
    public class PharmacyAdminCreateDto
    {
        public int PharmacyId { get; set; }
        public string AdminEmail { get; set; }
        public string AdminPassword { get; set; }
    }
}
