using AutoMapper;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Entities;

namespace Omini.Opme.Be.Api.Profiles;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<ItemCreateDto, Item>().ReverseMap();
        CreateMap<Item, ItemOutputDto>();
    }
}