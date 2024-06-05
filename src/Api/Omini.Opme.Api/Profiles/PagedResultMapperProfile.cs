using AutoMapper;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Api.Profiles;

public class PagedResultMapperProfile : Profile
{
    public PagedResultMapperProfile()
    {
        CreateMap(typeof(PagedResult<>), typeof(PagedResult<>))
            .ForCtorParam("source", opt => opt.MapFrom("Response"))
            .ForCtorParam("currentPage", opt => opt.MapFrom("CurrentPage"))
            .ForCtorParam("pageSize", opt => opt.MapFrom("PageSize"))
            .ForCtorParam("rowCount", opt => opt.MapFrom("RowCount"));
    }
}