using Medfast.Services.MedicationAPI.Models.Dto;
using Medfast.Services.MedicationAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Medfast.Services.MedicationAPI.Controllers
{
  [Route("api/medicines")]
  public class MedicineApiController : ControllerBase
  {
    protected ResponseDto  _response;
    private IMedicineRepository _medicineRepository;

    public MedicineApiController(IMedicineRepository medicineRepository)
    {
      _medicineRepository = medicineRepository;
      this._response = new ResponseDto();
    }

    [HttpGet]
    public async Task<object> Get()
    {
      try
      {
        IEnumerable<MedicineDto> medicineDtos = await _medicineRepository.GetMedicines();  
        _response.Result = medicineDtos;
      }
      catch (Exception ex)
      {

        _response.IsSuccess = false;
        _response.ErrorMessages = new List<string>() { ex.ToString() };
      }
      return _response; 
    }
  
    }
  }