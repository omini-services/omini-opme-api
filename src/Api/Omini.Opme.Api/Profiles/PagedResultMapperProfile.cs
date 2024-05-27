using AutoMapper;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Api.Profiles;

public class PagedResultMapperProfile : Profile
{
    public PagedResultMapperProfile()
    {
        CreateMap(typeof(PagedResult<>), typeof(PagedResult<>))
            .ForCtorParam("source", opt => opt.MapFrom("Response"))
            .ForCtorParam("pageNumber", opt => opt.MapFrom("PageNumber"))
            .ForCtorParam("pageSize", opt => opt.MapFrom("PageSize"))
            .ForCtorParam("totalCount", opt => opt.MapFrom("TotalCount"));
    }
}