using AutoMapper;
using Medfast.Services.MedicationAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Medfast.Services.MedicationAPI.Models;
using Medfast.Services.MedicationAPI.DbContexts;
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
    }

    public async Task<IEnumerable<PharmacyDto>> GetAllPharmacies()
    {
        var pharmacies = await _dbContext.Pharmacies.ToListAsync();

        return _mapper.Map<IEnumerable<PharmacyDto>>(pharmacies);
    }

    public async Task<PharmacyDto> GetPharmacyById(int id)
    {
        var pharmacy = await _dbContext.Pharmacies.FirstOrDefaultAsync(p => p.PharmacyId == id);

        return _mapper.Map<PharmacyDto>(pharmacy);
    }

    public async Task<bool> SaveChanges()
    {
        return (await _dbContext.SaveChangesAsync()) > 0;
    }
    
    
}