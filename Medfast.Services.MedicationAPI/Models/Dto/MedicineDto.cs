namespace Medfast.Services.MedicationAPI.Models.Dto
{
    public class MedicineDto
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string MedicineDescription { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public List<PharmacyMedicineDto> PharmacyMedicines { get; set; }
        public List<PharmacyDto> Pharmacies { get; set; }
    }

}
