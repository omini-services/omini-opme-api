using AutoMapper;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Domain.Warehouse;

namespace Omini.Opme.Be.Api.Profiles;

public class ItemMapperProfile : Profile
{
    public ItemMapperProfile()
    {
        CreateMap<Item, ItemOutputDto>();
    }
}