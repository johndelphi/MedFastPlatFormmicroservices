namespace Medfast.Services.MedicationAPI.Models.Dto;

public class PharmacyMedicineDto
{
    public int PharmacyId { get; set; }
    public string? PharmacyName { get; set; } 
    public double Price { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public double Latitude { get; set; }
    public double? MedicineDiscount { get; set; }
    public int QuantityInStock { get; set; }
    public double Longitude { get; set; }
    public double DistanceInKm { get; set; }
    public PharmacyDto Pharmacy { get; set; }
}

