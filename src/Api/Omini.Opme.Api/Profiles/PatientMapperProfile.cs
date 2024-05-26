using AutoMapper;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Domain.BusinessPartners;

namespace Omini.Opme.Api.Profiles;

public class PatientMapperProfile : Profile
{
    public PatientMapperProfile()
    {
        CreateMap<Patient, PatientOutputDto>()
            .ForPath(dest => dest.FirstName, opt => opt.MapFrom((src) => src.Name.FirstName))
            .ForPath(dest => dest.MiddleName, opt => opt.MapFrom((src) => src.Name.MiddleName))
            .ForPath(dest => dest.LastName, opt => opt.MapFrom((src) => src.Name.LastName));
    }
}