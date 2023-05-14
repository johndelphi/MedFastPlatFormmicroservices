using AutoMapper;
using Medfast.Services.MedicationAPI.Models;
using Medfast.Services.MedicationAPI.Models.Dto;
using Medfast.Services.MedicationAPI.Models.Dto.CreatePharmacyDto;

namespace Medfast.Services.MedicationAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<MedicineDto, Medicine>();
                config.CreateMap<Medicine, MedicineDto>()
                    .ForMember(dest => dest.PharmacyMedicines, opt => opt.MapFrom(src => src.PharmacyMedicines.Select(pm => new PharmacyMedicineDto 
                    { 
                        PharmacyId = pm.PharmacyId, 
                        Price = pm.Price, 
                        MedicineDiscount = pm.MedicineDiscount,
                        QuantityInStock = pm.QuantityInStock
                    }).ToList()));
                
                config.CreateMap<PharmacyDto, Pharmacy>();
                config.CreateMap<Pharmacy, PharmacyDto>();

                config.CreateMap<UserDto, User>();
                config.CreateMap<User, UserDto>();

                config.CreateMap<TransactionDto, Transaction>();
                config.CreateMap<Transaction, TransactionDto>();

                config.CreateMap<PharmacyMedicineDto, PharmacyMedicine>();
                config.CreateMap<PharmacyMedicine, PharmacyMedicineDto>()
                    .ForMember(dest => dest.PharmacyName, opt => opt.MapFrom(src => src.Pharmacy.PharmacyName))
                    .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Pharmacy.Latitude))
                    .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Pharmacy.Longitude))
                    .ForMember(dest => dest.Pharmacy, opt => opt.MapFrom(src => src.Pharmacy));

                config.CreateMap<PharmacyAccountCreateDto, Pharmacy>();
                config.CreateMap<Pharmacy, PharmacyAccountCreateDto>();
            });
            return mappingConfig;
        }
    }
}
