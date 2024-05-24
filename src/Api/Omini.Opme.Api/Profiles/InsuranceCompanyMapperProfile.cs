using AutoMapper;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Domain.BusinessPartners;

namespace Omini.Opme.Be.Api.Profiles;

public class InsuranceCompanyMapperProfile : Profile
{
    public InsuranceCompanyMapperProfile()
    {
        CreateMap<InsuranceCompany, InsuranceCompanyOutputDto>()
            .ForPath(dest => dest.LegalName, opt => opt.MapFrom((src) => src.Name.LegalName))
            .ForPath(dest => dest.TradeName, opt => opt.MapFrom((src) => src.Name.TradeName));
    }
}