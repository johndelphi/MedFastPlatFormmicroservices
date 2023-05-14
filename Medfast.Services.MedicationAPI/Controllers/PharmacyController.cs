using Microsoft.AspNetCore.Mvc;
using Medfast.Services.MedicationAPI.Models;
using AutoMapper;

using Medfast.Services.MedicationAPI.Models.Dto;

using Medfast.Services.MedicationAPI.Repository.PharmacyRepository;
using Medfast.Services.MedicationAPI.Models.Dto.CreatePharmacyDto;
using Medfast.Services.MedicationAPI.Models.Dto.InventoryDto;
using Medfast.Services.MedicationAPI.Repository;


namespace Medfast.Services.MedicationAPI.Controllers;


    [ApiController]
        [Route("api/[controller]")]
        public class PharmacyController : ControllerBase
        {
            private readonly IMapper _mapper;
            private readonly IPharmacyRepository _pharmacyRepository;
            
            private readonly IMedicineRepository _medicineRepository; 
            public PharmacyController(IMapper mapper, IPharmacyRepository pharmacyRepository, IMedicineRepository medicineRepository)   
            {
                _mapper = mapper;
                _pharmacyRepository = pharmacyRepository;
                _medicineRepository = medicineRepository;
            }
         
            [HttpPost]
            public async Task<IActionResult> CreatePharmacy(PharmacyAccountCreateDto pharmacyForCreationDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if a pharmacy with the same name and phone number already exists
                var existingPharmacy = await _pharmacyRepository.GetPharmacyByNameAndPhoneNumber(pharmacyForCreationDto.PharmacyName, pharmacyForCreationDto.PhoneNumber);

                if (existingPharmacy != null)
                {
                    // A pharmacy with the same name and phone number already exists
                    return Conflict("A pharmacy with the same name and phone number already exists.");
                }

                var pharmacy = new Pharmacy
                {
                    PharmacyName = pharmacyForCreationDto.PharmacyName,
                    Region = pharmacyForCreationDto.Region,
                    City = pharmacyForCreationDto.City,
                    SubCity = pharmacyForCreationDto.SubCity,
                    Landmark = pharmacyForCreationDto.Landmark,
                    PhoneNumber = pharmacyForCreationDto.PhoneNumber,
                    Latitude = pharmacyForCreationDto.Latitude,
                    Longitude = pharmacyForCreationDto.Longitude
                };

                await _pharmacyRepository.CreatePharmacy(pharmacy);

                return Ok();
            }
            
            [HttpPost("pharmacies/{pharmacyId}/medicines")]
            public async Task<IActionResult> AddMedicineToInventory(int pharmacyId, PharmacyMedicineCreateDto pharmacyMedicineCreateDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if the pharmacy exists
                var pharmacy = await _pharmacyRepository.GetPharmacyById(pharmacyId);
                if (pharmacy == null)
                {
                    return NotFound(new { message = $"Pharmacy with ID {pharmacyId} not found" });
                }

                // Check if the medicine already exists in the pharmacy's inventory
                var existingPharmacyMedicine = await _pharmacyRepository.GetPharmacyMedicineByPharmacyIdAndMedicineName(pharmacyId, pharmacyMedicineCreateDto.MedicineName);
                if (existingPharmacyMedicine != null)
                {
                    return BadRequest(new { message = "Medicine already exists in the pharmacy's inventory" });
                }

                // Retrieve the medicine ID based on the medicine name
                var medicine = await _medicineRepository.GetMedicineByNameph(pharmacyMedicineCreateDto.MedicineName);
                if (medicine == null)
                {
                    return NotFound(new { message = $"Medicine with name '{pharmacyMedicineCreateDto.MedicineName}' not found" });
                }

                // Map the PharmacyMedicineCreateDto object to the PharmacyMedicine model
                var pharmacyMedicine = new PharmacyMedicine
                {
                    MedicineId = medicine.MedicineId,
                    PharmacyId = pharmacyId,
                    Price = pharmacyMedicineCreateDto.Price,
                    MedicineDiscount = pharmacyMedicineCreateDto.MedicineDiscount,
                    QuantityInStock = pharmacyMedicineCreateDto.QuantityInStock
                };

                await _pharmacyRepository.AddMedicineToPharmacyInventory(pharmacyId , pharmacyMedicineCreateDto);

                return CreatedAtRoute("GetPharmacyMedicineById", new { pharmacyId = pharmacyMedicine.PharmacyId, medicineId = pharmacyMedicine.MedicineId }, pharmacyMedicine);
            }

            



          
           }
                   
