using AutoMapper;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Domain.Sales;

namespace Omini.Opme.Api.Profiles;

public class QuotationMapperProfile : Profile
{
    public QuotationMapperProfile()
    {
        CreateMap<Quotation, QuotationOutputDto>();
        CreateMap<QuotationItem, QuotationOutputDto.QuotationOutputItemDto>();
    }
}