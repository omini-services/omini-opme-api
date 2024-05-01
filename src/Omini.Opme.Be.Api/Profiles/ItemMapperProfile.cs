using AutoMapper;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Domain.Entities;

namespace Omini.Opme.Be.Api.Profiles;

public class ItemMapperProfile : Profile
{
    public ItemMapperProfile()
    {
        CreateMap<ItemCreateDto, Item>();
        CreateMap<Item, ItemOutputDto>();
    }
}