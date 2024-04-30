using AutoMapper;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Domain.Entities;

namespace Omini.Opme.Be.Api.Profiles;

public class PatientMapperProfile : Profile
{
    public PatientMapperProfile()
    {
        CreateMap<PatientCreateDto, Patient>()
            .ForPath(dest => dest.Name.FirstName, opt => opt.MapFrom((src) => src.FirstName))
            .ForPath(dest => dest.Name.MiddleName, opt => opt.MapFrom((src) => src.MiddleName))
            .ForPath(dest => dest.Name.LastName, opt => opt.MapFrom((src) => src.LastName));
        CreateMap<Patient, PatientOutputDto>()
            .ForPath(dest => dest.FirstName, opt => opt.MapFrom((src) => src.Name.FirstName))
            .ForPath(dest => dest.MiddleName, opt => opt.MapFrom((src) => src.Name.MiddleName))
            .ForPath(dest => dest.LastName, opt => opt.MapFrom((src) => src.Name.LastName));
    }
}