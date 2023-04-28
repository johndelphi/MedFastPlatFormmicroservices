namespace Medfast.Services.MedicationAPI.Models
{
  public class Pharmacy
  {
    public int PharmacyId { get; set; }
    public string PharmacyName { get; set; }
    public ICollection<PharmacyPrice> PharmacyPrices { get; set; }
  }
}
