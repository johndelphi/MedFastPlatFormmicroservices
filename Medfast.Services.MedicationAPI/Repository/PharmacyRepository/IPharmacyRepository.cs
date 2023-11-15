using System.Collections.Generic;
using System.Threading.Tasks;
using Medfast.Services.MedicationAPI.Models;
using Medfast.Services.MedicationAPI.Models.Dto;
using Medfast.Services.MedicationAPI.Models.Dto.InventoryDto;

namespace Medfast.Services.MedicationAPI.Repository.PharmacyRepository
{
    public interface IPharmacyRepository
    {
        Task<IEnumerable<PharmacyDto>> GetAllPharmacies();
        Task AddPharmacy(PharmacyDto pharmacyDto);
        Task CreatePharmacy(Pharmacy pharmacy);
        Task<(Pharmacy Pharmacy, User AdminUser, string ErrorMessage)> CreatePharmacyWithAdmin(Pharmacy pharmacy, User adminUser);
        Task<Pharmacy?> GetPharmacyByNameAndPhoneNumber(string pharmacyName, string phoneNumber);
        Task AddMedicineToPharmacyInventory(int pharmacyId, PharmacyMedicineCreateDto pharmacyMedicineCreateDto);
        Task<Pharmacy> GetPharmacyById(int pharmacyId);
        Task<PharmacyMedicine?> GetPharmacyMedicineByPharmacyIdAndMedicineId(int pharmacyId, int medicineId);
        Task<bool> SaveChanges();
        Task<PharmacyMedicine?> GetPharmacyMedicineByPharmacyIdAndMedicineName(int pharmacyId, string medicineName);

        Task CreateAdminUser(User adminUser);
    }
}