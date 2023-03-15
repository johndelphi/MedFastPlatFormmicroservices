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

    [HttpGet]
    [Route("{id}")]
    public async Task<object> Get(int id)
    {
        try
        {
            MedicineDto medicineDto = await _medicineRepository.GetMedicineById(id);
            _response.Result = medicineDto;
        }
        catch (Exception ex)
        {

            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }



    [HttpPost]
    public async Task<object> Post([FromBody] MedicineDto medicineDto)
    {
        try
        {
            MedicineDto model = await _medicineRepository.AddMedicine(medicineDto);
            _response.Result = model;
        }
        catch (Exception ex)
        {

            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }

    [HttpPut]
    public async Task<object> Put([FromBody] MedicineDto medicineDto)
    {
        try
        {
            MedicineDto model = await _medicineRepository.AddMedicine(medicineDto);
            _response.Result = model;
        }
        catch (Exception ex)
        {

            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }

    [HttpDelete]
    public async Task<object> Delete(int id)
    {
        try
        {
            bool isSucces = await _medicineRepository.DeleteMedicine(id);
            _response.Result = isSucces;
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