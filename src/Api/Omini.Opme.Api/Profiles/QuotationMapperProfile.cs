using AutoMapper;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Domain.Sales;

namespace Omini.Opme.Api.Profiles;

public class QuotationMapperProfile : Profile
{
    public QuotationMapperProfile()
    {
        CreateMap<Quotation, QuotationOutputDto>()
            .ForMember(dest => dest.PatientFirstName, opt => opt.MapFrom(src => src.PatientName.FirstName))
            .ForMember(dest => dest.PatientMiddleName, opt => opt.MapFrom(src => src.PatientName.MiddleName))
            .ForMember(dest => dest.PatientLastName, opt => opt.MapFrom(src => src.PatientName.LastName))
            .ForMember(dest => dest.PhysicianFirstName, opt => opt.MapFrom(src => src.PhysicianName.FirstName))
            .ForMember(dest => dest.PhysicianMiddleName, opt => opt.MapFrom(src => src.PhysicianName.MiddleName))
            .ForMember(dest => dest.PhysicianLastName, opt => opt.MapFrom(src => src.PhysicianName.LastName))
            .ForMember(dest => dest.HospitalName, opt => opt.MapFrom(src => src.HospitalName))
            .ForMember(dest => dest.InsuranceCompanyName, opt => opt.MapFrom(src => src.InsuranceCompanyName))
            .ForMember(dest => dest.PayingSourceName, opt => opt.MapFrom(src => src.PayingSourceName));
        CreateMap<QuotationItem, QuotationOutputDto.QuotationOutputItemDto>();
    }
}