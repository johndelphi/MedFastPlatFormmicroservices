using Medfast.Services.MedicationAPI.Models;
using Medfast.Services.MedicationAPI.Models.Dto;
using Medfast.Services.MedicationAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Medfast.Services.MedicationAPI.Controllers
{
  [Route("api/medicines")]
  public class MedicineApiController : ControllerBase
  {
    protected new readonly ResponseDto  Response;
    private IMedicineRepository _medicineRepository;
    
    public MedicineApiController(IMedicineRepository medicineRepository)
    {
      _medicineRepository = medicineRepository;
      this.Response = new ResponseDto();
    }

    [HttpGet]
    public async Task<object> Get()
    {
      try
      {
        IEnumerable<MedicineDto> medicineDtos = await _medicineRepository.GetMedicines();  
        Response.Result = medicineDtos;
      }
      catch (Exception ex)
      {

        Response.IsSuccess = false;
        Response.ErrorMessages = new List<string>() { ex.ToString() };
      }
      return Response; 
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<object> Get(int id)
    {
        try
        {
            MedicineDto medicineDto = await _medicineRepository.GetMedicineById(id);
            Response.Result = medicineDto;
        }
        catch (Exception ex)
        {

            Response.IsSuccess = false;
            Response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return Response;
    }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<MedicineDto>>> SearchMedicines(string name)
        {
            var medicines = await _medicineRepository.GetMedicineByName(name);
            return Ok(medicines);
        }

        [HttpPut("{medicineId}")]
        public async Task<IActionResult> UpdateMedicine(int medicineId, [FromBody] Medicine medicine)
        {
            try
            {
                if (medicineId != medicine.MedicineId)
                {
                    return BadRequest("Invalid medicine ID");
                }

                var medicineDto = new MedicineDto
                {
                    MedicineId = medicine.MedicineId,
                    MedicineName = medicine.MedicineName,
                    MedicineDescription = medicine.MedicineDescription,
                    Category = medicine.Category,
                    ImageUrl = medicine.ImageUrl
                };

                var updatedMedicine = await _medicineRepository.UpdateMedicine(medicineDto);

                return Ok(updatedMedicine);
            }
            catch (Exception ex)
            {
             
                return StatusCode(500, $"An error occurred while updating: {ex.Message}");
            }
        }

       


    [HttpDelete]
    public async Task<object> Delete(int id)
    {
        try
        {
            bool isSucces = await _medicineRepository.DeleteMedicine(id);
            Response.Result = isSucces;
        }
        catch (Exception ex)
        {

            Response.IsSuccess = false;
            Response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return Response;
    }
    }


  }