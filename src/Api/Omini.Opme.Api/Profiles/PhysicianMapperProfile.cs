using AutoMapper;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Domain.BusinessPartners;

namespace Omini.Opme.Be.Api.Profiles;

public class PhysicianMapperProfile : Profile
{
    public PhysicianMapperProfile()
    {
        CreateMap<Physician, PhysicianOutputDto>()
            .ForPath(dest => dest.FirstName, opt => opt.MapFrom((src) => src.Name.FirstName))
            .ForPath(dest => dest.MiddleName, opt => opt.MapFrom((src) => src.Name.MiddleName))
            .ForPath(dest => dest.LastName, opt => opt.MapFrom((src) => src.Name.LastName));
    }
}