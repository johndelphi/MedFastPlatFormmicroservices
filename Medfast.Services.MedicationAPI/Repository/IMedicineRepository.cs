using Medfast.Services.MedicationAPI.Models.Dto;
namespace Medfast.Services.MedicationAPI.Repository
{
  public interface IMedicineRepository
  {
    Task<IEnumerable<MedicineDto>>GetMedicineById(int medicineid);
    Task<IEnumerable<MedicineDto>> GetMedicines();
    Task<MedicineDto> AddMedicine(MedicineDto medicineDto);
    Task<bool> DeleteMedicine(int  medicineId );
    
  }
}
