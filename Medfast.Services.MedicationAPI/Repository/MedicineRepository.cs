using AutoMapper;
using Medfast.Services.MedicationAPI.DbContexts;
using Medfast.Services.MedicationAPI.Models;

using Medfast.Services.MedicationAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Medfast.Services.MedicationAPI.Repository
{
  public class MedicineRepository : IMedicineRepository
  {
    private readonly ApplicationDbContext _db;
    private readonly ILogger<MedicineRepository> _logger;
    private IMapper _mapper;
    
    
    public MedicineRepository(ApplicationDbContext db,IMapper mapper,ILogger<MedicineRepository> logger)
    {
      _db = db; 
      _mapper = mapper;
      _logger = logger;

    }

    //public async Task<MedicineDto> AddMedicine(MedicineDto medicineDto)
    //{
    // Medicine medicine = _mapper.Map<MedicineDto, Medicine>(medicineDto);
    //  if (medicine.MedicineId > 0)
    //  {
    //    _db.Medicines.Update(medicine);
    //  }
    //  else
    //  {
    //    _db.Medicines.Add(medicine);
    //  }
    //  await _db.SaveChangesAsync();
    //  return _mapper.Map<Medicine, MedicineDto>(medicine);
    //}

    public async Task<bool> DeleteMedicine(int medicineId)
    {
      try
      {
        Medicine? medicine = await _db.Medicines.FirstOrDefaultAsync(u => u != null && u.MedicineId == medicineId);
        if(medicine == null)
        {
          return false;
        }
        _db.Medicines.Remove(medicine);
        await _db.SaveChangesAsync();
        return true;
      }
      catch (Exception)
      {

        return false;
      }
     
    }

        //public  async Task<MedicineDto> GetMedicineById(int medicineId)
        //{
        // Medicine medicine = await _db.Medicines.Where(m => m.MedicineId == medicineId).FirstOrDefaultAsync();
        //  return _mapper.Map<MedicineDto>(medicine);
        //}

        public async Task<IEnumerable<MedicineDto>> GetMedicineByName(string name)
        {
            try
            {
                var medicines = await _db.Medicines
                    .Include(m => m.PharmacyMedicines)
                    .ThenInclude(pm => pm.Pharmacy)
                    .Where(m => m.MedicineName.ToLower().Contains(name.ToLower()))
                    .ToListAsync();

                var medicineDtos = _mapper.Map<List<MedicineDto>>(medicines);

                foreach (var medicineDto in medicineDtos)
                {
                    var pharmacyDtos = medicineDto.PharmacyMedicines
                        .Where(pm => pm.QuantityInStock > 0)
                        .Select(pm => new PharmacyDto
                        {
                            PharmacyId = pm.Pharmacy.PharmacyId,
                            PharmacyName = pm.Pharmacy.PharmacyName,
                            Region = pm.Pharmacy.Region,
                            City = pm.Pharmacy.City,
                            SubCity = pm.Pharmacy.SubCity,
                            Landmark = pm.Pharmacy.Landmark,
                            Latitude = pm.Pharmacy.Latitude,
                            Longitude = pm.Pharmacy.Longitude,
                            DistanceInKm = 0 // Set to 0 for now, but you can calculate this based on the user's location
                        })
                        .ToList();

                    medicineDto.Pharmacies = pharmacyDtos;
                }

                return medicineDtos;
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, $"An error occurred while getting medicines by name: {name}");

                // Rethrow the exception to be handled by a higher level
                throw;
            }
        }

        public async Task<IEnumerable<MedicineDto>> GetMedicines()
        {


            List<Medicine?> medicineList = await _db.Medicines.ToListAsync();
            return _mapper.Map<List<MedicineDto>>(medicineList);

        }

        public async Task<MedicineDto> GetProductsById(int medicineId)
        {
            Medicine medicine = await _db.Medicines.Where(x => x.MedicineId == medicineId).FirstOrDefaultAsync();

            return _mapper.Map<MedicineDto>(medicine);

        }

        public async Task<MedicineDto> AddMedicine(MedicineDto medicineDto)
        {
            var medicine = _mapper.Map<Medicine>(medicineDto);

            _db.Medicines.Add(medicine);
            await _db.SaveChangesAsync();

            return _mapper.Map<MedicineDto>(medicine);
        }


        async Task<bool> IMedicineRepository.DeleteMedicine(int medicineId)
    {
        try
        {
            Medicine? medicine = await _db.Medicines.FirstOrDefaultAsync(u => u.MedicineId == medicineId);
            if (medicine == null)
            {
                return false;
            }

            _db.Medicines.Remove(medicine);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception )
        {
            return false;
        }
    }

    public async Task<MedicineDto> GetMedicineById(int medicineid)
    {
        Medicine medicine = await _db.Medicines.Where(x => x.MedicineId == medicineid).FirstOrDefaultAsync();
        return _mapper.Map<MedicineDto>(medicine);
    }

    //public async Task<MedicineDto> GetMedicineByName(string name)
    //{
    //  var medicine = await _db.Medicines.FirstOrDefaultAsync(m => m.MedicineName == name);
    //  return _mapper.Map<MedicineDto>(medicine);
    //}


  }
}
