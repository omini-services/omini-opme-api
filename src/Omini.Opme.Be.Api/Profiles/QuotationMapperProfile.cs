using AutoMapper;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Domain.Entities;

namespace Omini.Opme.Be.Api.Profiles;

public class QuotationMapperProfile : Profile
{
    public QuotationMapperProfile()
    {
        CreateMap<Quotation, QuotationOutputDto>();
        CreateMap<QuotationItem, QuotationOutputItemDto>();
    }
}