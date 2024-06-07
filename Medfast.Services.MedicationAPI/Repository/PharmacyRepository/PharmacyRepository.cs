using AutoMapper;
using Medfast.Services.MedicationAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Medfast.Services.MedicationAPI.Models;
using Medfast.Services.MedicationAPI.DbContexts;
using Medfast.Services.MedicationAPI.Models.Dto.InventoryDto;
using Medfast.Services.MedicationAPI.Models.Dto.CreatePharmacyDto;
using Microsoft.Extensions.Logging;

namespace Medfast.Services.MedicationAPI.Repository.PharmacyRepository;

public class PharmacyRepository : IPharmacyRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<PharmacyRepository> _logger;

    public PharmacyRepository(ApplicationDbContext dbContext, IMapper mapper, ILogger<PharmacyRepository> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

    public async Task CreateAdminUser(User adminUser)
    {
        // _dbContext.Users.Add(adminUser);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Pharmacy?> GetPharmacyById(int pharmacyId)
    {
        var pharmacy = await _dbContext.Pharmacies
            .Select(p => new Pharmacy
            {
                PharmacyId = p.PharmacyId,
                PharmacyName = p.PharmacyName ?? string.Empty,
                PhoneNumber = p.PhoneNumber ?? string.Empty,
                City = p.City ?? string.Empty,
                SubCity = p.SubCity ?? string.Empty,
                Region = p.Region ?? string.Empty,
                Landmark = p.Landmark ?? string.Empty,
                Latitude = p.Latitude,
                Longitude = p.Longitude
            })
            .FirstOrDefaultAsync(p => p.PharmacyId == pharmacyId);

        if (pharmacy == null)
        {
            _logger.LogWarning("Pharmacy with ID {PharmacyId} not found.", pharmacyId);
        }

        return pharmacy;
    }

    public async Task<Pharmacy?> GetPharmacyByNameAndPhoneNumber(string pharmacyName, string phoneNumber)
    {
        return await _dbContext.Pharmacies
            .FirstOrDefaultAsync(p => p.PharmacyName == pharmacyName || p.PhoneNumber == phoneNumber);
    }

    #region Inventory

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

    async Task<(Pharmacy Pharmacy, User AdminUser, string ErrorMessage)> IPharmacyRepository.CreatePharmacyWithAdmin(Pharmacy pharmacy, User adminUser)
    {
        using var transaction = _dbContext.Database.BeginTransaction();

        try
        {
            // Check for an existing pharmacy with the same name and/or phone number
            var existingPharmacy = await _dbContext.Pharmacies
                .AnyAsync(p => p.PhoneNumber == pharmacy.PhoneNumber);
            if (existingPharmacy)
            {
                return (null, null, "A pharmacy with the same phone number already exists.");
            }

            // Add the pharmacy to the DbContext
            _dbContext.Pharmacies.Add(pharmacy);
            await _dbContext.SaveChangesAsync();

            // Assign the newly created pharmacy ID to the admin user and add the user to the DbContext
            adminUser.PharmacyId = pharmacy.PharmacyId;
            // _dbContext.Users.Add(adminUser);
            await _dbContext.SaveChangesAsync();

            // Commit the transaction
            await transaction.CommitAsync();

            return (pharmacy, adminUser, null);
        }
        catch (Exception ex)
        {
            // Rollback the transaction if any exception occurs
            await transaction.RollbackAsync();
            // Log the exception here as needed
            return (null, null, $"An error occurred while creating the pharmacy and admin user: {ex.Message}");
        }
    }
}