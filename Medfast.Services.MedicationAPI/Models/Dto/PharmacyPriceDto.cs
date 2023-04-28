namespace Medfast.Services.MedicationAPI.Models.Dto
{
  public class PharmacyPriceDto
  {
    public int PharmacyPriceId { get; set; }
    public int PharmacyId { get; set; }
    public PharmacyDto Pharmacy { get; set; }
    public int MedicineId { get; set; }
    public MedicineDto Medicine { get; set; }
    public double Price { get; set; }
  }
}
