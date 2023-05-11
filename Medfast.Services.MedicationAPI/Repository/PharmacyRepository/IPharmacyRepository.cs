using Medfast.Services.MedicationAPI.Models.Dto;

namespace Medfast.Services.MedicationAPI.Repository.PharmacyRepository;

public interface IPharmacyRepository
{
    Task<PharmacyDto> GetPharmacyById(int id);
    Task<IEnumerable<PharmacyDto>> GetAllPharmacies();
    Task AddPharmacy(PharmacyDto pharmacyDto);
    Task<bool> SaveChanges();  
}