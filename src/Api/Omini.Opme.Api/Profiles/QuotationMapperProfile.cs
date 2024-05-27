using AutoMapper;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Domain.Sales;

namespace Omini.Opme.Api.Profiles;

public class QuotationMapperProfile : Profile
{
    public QuotationMapperProfile()
    {
        CreateMap<Quotation, QuotationOutputDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.Name.FullName))
            .ForMember(dest => dest.HospitalName, opt => opt.MapFrom(src => src.Hospital.Name.LegalName))
            .ForMember(dest => dest.InsuranceCompanyName, opt => opt.MapFrom(src => src.InsuranceCompany.Name.LegalName))
            .ForMember(dest => dest.PhysicianName, opt => opt.MapFrom(src => src.Physician.Name.FullName))
            .ForMember(dest => dest.PayingSourceName, opt => opt.MapFrom(src => src.PayingSource.Name));
        CreateMap<QuotationItem, QuotationOutputDto.QuotationOutputItemDto>();
    }
}