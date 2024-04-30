using AutoMapper;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Domain.Entities;

namespace Omini.Opme.Be.Api.Profiles;

public class HospitalMapperProfile : Profile
{
    public HospitalMapperProfile()
    {
        CreateMap<HospitalCreateDto, Hospital>()
            .ForPath(dest => dest.Name.LegalName, opt => opt.MapFrom((src) => src.LegalName))
            .ForPath(dest => dest.Name.TradeName, opt => opt.MapFrom((src) => src.TradeName));
        CreateMap<Hospital, HospitalOutputDto>()
            .ForPath(dest => dest.LegalName, opt => opt.MapFrom((src) => src.Name.LegalName))
            .ForPath(dest => dest.TradeName, opt => opt.MapFrom((src) => src.Name.TradeName));
    }
}