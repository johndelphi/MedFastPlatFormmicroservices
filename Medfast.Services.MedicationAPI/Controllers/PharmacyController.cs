using Microsoft.AspNetCore.Mvc;
using Medfast.Services.MedicationAPI.Models;
using AutoMapper;

using Medfast.Services.MedicationAPI.Models.Dto;

using Medfast.Services.MedicationAPI.Repository.PharmacyRepository;
using Medfast.Services.MedicationAPI.Models.Dto.CreatePharmacyDto;


namespace Medfast.Services.MedicationAPI.Controllers;


    [ApiController]
        [Route("api/[controller]")]
        public class PharmacyController : ControllerBase
        {
            private readonly IMapper _mapper;
            private readonly IPharmacyRepository _pharmacyRepository;
    
            public PharmacyController(IMapper mapper, IPharmacyRepository pharmacyRepository)
            {
                _mapper = mapper;
                _pharmacyRepository = pharmacyRepository;
            }
         
[HttpPost]
public async Task<IActionResult> CreatePharmacy(PharmacyAccountCreateDto pharmacyForCreationDto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var pharmacy = _mapper.Map<Pharmacy>(pharmacyForCreationDto);

    await _pharmacyRepository.AddPharmacy(pharmacy);

    var pharmacyDto = _mapper.Map<PharmacyDto>(pharmacy);

    return CreatedAtRoute("GetPharmacyById", new { pharmacyId = pharmacyDto.PharmacyId }, pharmacyDto);
}

    

          
           }
                   
