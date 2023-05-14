using AutoMapper;
using Medfast.Services.MedicationAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Medfast.Services.MedicationAPI.Models;
using Medfast.Services.MedicationAPI.DbContexts;
using Medfast.Services.MedicationAPI.Models.Dto.InventoryDto;

namespace Medfast.Services.MedicationAPI.Repository.PharmacyRepository;

public class PharmacyRepository: IPharmacyRepository
{
    private readonly  ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public PharmacyRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task AddPharmacy(PharmacyDto pharmacyDto)
    {
        var pharmacy = _mapper.Map<Pharmacy>(pharmacyDto);

        _dbContext.Pharmacies.Add(pharmacy);

        await _dbContext.SaveChangesAsync();
        pharmacy.PharmacyId = pharmacyDto.PharmacyId;
    }

    public async Task<IEnumerable<PharmacyDto>> GetAllPharmacies()
    {
        var pharmacies = await _dbContext.Pharmacies.ToListAsync();

        return _mapper.Map<IEnumerable<PharmacyDto>>(pharmacies);
    }

    
    
    public async Task CreatePharmacy(Pharmacy pharmacy)
    {
        _dbContext.Pharmacies.Add(pharmacy);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<Pharmacy> GetPharmacyById(int pharmacyId)
    {
        return await _dbContext.Pharmacies.FindAsync(pharmacyId);
    }
    public async Task<Pharmacy?> GetPharmacyByNameAndPhoneNumber(string pharmacyName, string phoneNumber)
    {
        return await _dbContext.Pharmacies
            .FirstOrDefaultAsync(p => p.PharmacyName == pharmacyName || p.PhoneNumber == phoneNumber);
    }

    #region inventory

    public async Task AddMedicineToPharmacyInventory(int pharmacyId, PharmacyMedicineCreateDto pharmacyMedicineCreateDto)
    {
        var pharmacy = await _dbContext.Pharmacies.FirstOrDefaultAsync(p => p.PharmacyId == pharmacyId);
        if (pharmacy == null)
        {
            throw new Exception($"Pharmacy with ID {pharmacyId} not found.");
        }

        var medicine = await _dbContext.Medicines.FirstOrDefaultAsync(m => m.MedicineName == pharmacyMedicineCreateDto.MedicineName);
        if (medicine == null)
        {
            throw new Exception($"Medicine with name '{pharmacyMedicineCreateDto.MedicineName}' not found.");
        }

        var pharmacyMedicine = new PharmacyMedicine
        {
            MedicineId = medicine.MedicineId,
            PharmacyId = pharmacyId,
            Price = pharmacyMedicineCreateDto.Price,
            MedicineDiscount = pharmacyMedicineCreateDto.MedicineDiscount,
            QuantityInStock = pharmacyMedicineCreateDto.QuantityInStock
            
        };

        _dbContext.PharmacyMedicines.Add(pharmacyMedicine);
        await _dbContext.SaveChangesAsync();
    }


    public async Task<PharmacyMedicine?> GetPharmacyMedicineByPharmacyIdAndMedicineId(int pharmacyId, int medicineId)
    {
        return await _dbContext.PharmacyMedicines
            .Include(pm => pm.Medicine)
            .SingleOrDefaultAsync(pm => pm.PharmacyId == pharmacyId && pm.MedicineId == medicineId);
    }
    
    public async Task<PharmacyMedicine?> GetPharmacyMedicineByPharmacyIdAndMedicineName(int pharmacyId, string medicineName)
    {
        return await _dbContext.PharmacyMedicines
            .Include(pm => pm.Medicine)
            .FirstOrDefaultAsync(pm => pm.PharmacyId == pharmacyId && pm.Medicine.MedicineName == medicineName);
    }
    
    #endregion
    public async Task<bool> SaveChanges()
    {
        return (await _dbContext.SaveChangesAsync()) > 0;
    }
    
    
}