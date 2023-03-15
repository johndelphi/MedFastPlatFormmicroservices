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
    private IMapper _mapper;
    public MedicineRepository(ApplicationDbContext db,IMapper mapper)
    {
      _db = db; 
      _mapper = mapper;

    }

    public async Task<MedicineDto> AddMedicine(MedicineDto medicineDto)
    {
     Medicine medicine = _mapper.Map<MedicineDto, Medicine>(medicineDto);
      if (medicine.MedicineId > 0)
      {
        _db.Medicines.Update(medicine);
      }
      else
      {
        _db.Medicines.Add(medicine);
      }
      await _db.SaveChangesAsync();
      return _mapper.Map<Medicine, MedicineDto>(medicine);
    }

    public async Task<bool> DeleteMedicine(int medicineId)
    {
      try
      {
        Medicine medicine = await _db.Medicines.FirstOrDefaultAsync(u => u.MedicineId == medicineId);
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

        //public async Task<IEnumerable<MedicineDto>> GetMedicineByName(string name)
        //{
        //  var medicines = await _db.Medicines
        //    .Where(m => m.MedicineName.ToLower().Contains(name.ToLower()))
        //    .ToListAsync();
        //  var medicineDtos = _mapper.Map<List<MedicineDto>>(medicines);
        //  return medicineDtos;
        //}

        public async Task<IEnumerable<MedicineDto>> GetMedicines()
        {


            List<Medicine> medicineList = await _db.Medicines.ToListAsync();
            return _mapper.Map<List<MedicineDto>>(medicineList);

        }

        public async Task<MedicineDto> GetProductsById(int medicineId)
        {
            Medicine medicine = await _db.Medicines.Where(x => x.MedicineId == medicineId).FirstOrDefaultAsync();

            return _mapper.Map<MedicineDto>(medicine);

        }

        async Task <MedicineDto>IMedicineRepository.AddMedicine(MedicineDto medicineDto)
    {
      Medicine medicine = _mapper.Map<MedicineDto,Medicine>(medicineDto);
      if (medicine.MedicineId > 0)
      {
          _db.Medicines.Update(medicine);
      }

      await _db.SaveChangesAsync();

      return  _mapper.Map<Medicine,MedicineDto>(medicine);
    }

    async Task<bool> IMedicineRepository.DeleteMedicine(int medicineId)
    {
        try
        {
            Medicine medicine = await _db.Medicines.FirstOrDefaultAsync(u => u.MedicineId == medicineId);
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

    public async Task<MedicineDto> GetMedicineByName(string name)
    {
      var medicine = await _db.Medicines.FirstOrDefaultAsync(m => m.MedicineName == name);
      return _mapper.Map<MedicineDto>(medicine);
    }


  }
}
