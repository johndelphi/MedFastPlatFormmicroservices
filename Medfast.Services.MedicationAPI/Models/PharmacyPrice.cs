namespace Medfast.Services.MedicationAPI.Models
{
  public class PharmacyPrice
  {
    public int PharmacyPriceId { get; set; }
    public int PharmacyId { get; set; }
    public Pharmacy Pharmacy { get; set; }
    public int MedicineId { get; set; }
    public Medicine Medicine { get; set; }
    public double Price { get; set; }
  }
}
