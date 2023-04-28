namespace Medfast.Services.MedicationAPI.Models.Dto
{
  public class PharmacyMedicineDto
  {
    public int MedicineId { get; set; }
    public string MedicineName { get; set; }
    public string MedicineDescription { get; set; }
    public double Price { get; set; }
    public string Category { get; set; }
    public string ImageUrl { get; set; }
    public int PharmacyId { get; set; }
    public string PharmacyName { get; set; } 
  }
}
