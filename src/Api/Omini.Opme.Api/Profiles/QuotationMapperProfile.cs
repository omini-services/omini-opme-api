using AutoMapper;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Domain.Sales;

namespace Omini.Opme.Api.Profiles;

public class QuotationMapperProfile : Profile
{
    public QuotationMapperProfile()
    {
        CreateMap<Quotation, QuotationOutputDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.PatientName))
            .ForMember(dest => dest.HospitalName, opt => opt.MapFrom(src => src.HospitalName))
            .ForMember(dest => dest.InsuranceCompanyName, opt => opt.MapFrom(src => src.InsuranceCompanyName))
            .ForMember(dest => dest.PhysicianName, opt => opt.MapFrom(src => src.PhysicianName))
            .ForMember(dest => dest.PayingSourceName, opt => opt.MapFrom(src => src.PayingSourceName));
        CreateMap<QuotationItem, QuotationOutputDto.QuotationOutputItemDto>();
    }
}