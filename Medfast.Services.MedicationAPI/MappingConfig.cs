using AutoMapper;
using Medfast.Services.MedicationAPI.Models;
using Medfast.Services.MedicationAPI.Models.Dto;

namespace Medfast.Services.MedicationAPI
{
  public class MappingConfig
  {
    public static MapperConfiguration RegisterMaps()
    {
      var mappingCofig = new MapperConfiguration(config => {
       
        config.CreateMap<MedicineDto, Medicine>();
        config.CreateMap<Medicine, MedicineDto>();
      });
      return mappingCofig;

    }
  }
}
