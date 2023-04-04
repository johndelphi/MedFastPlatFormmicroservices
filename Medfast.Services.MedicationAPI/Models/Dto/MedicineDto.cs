namespace Medfast.Services.MedicationAPI.Models.Dto
{
  public class MedicineDto
  {
    
    

    public string MedicineName { get; set; }

    public string MedicineDescription { get; set; }

    public  double MedicineDiscount { get; set; }


    public double price { get; set; }

    public string Category { get; set; }

    public string ImageUrl { get; set; }
  }
}
