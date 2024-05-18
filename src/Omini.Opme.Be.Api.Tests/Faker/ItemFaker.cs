using Bogus;
using FluentAssertions.Extensions;
using Omini.Opme.Be.Api.Dtos;

namespace Omini.Opme.Be.Api.Tests;

public static class ItemFaker
{
    public static Faker<ItemCreateDto> GetFakerItemCreateDto()
    {
        return new Faker<ItemCreateDto>()
            .RuleFor(o => o.Code, f => f.Random.AlphaNumeric(8))
            .RuleFor(o => o.Name, f => f.Commerce.ProductName())
            .RuleFor(o => o.SalesName, f => f.Commerce.ProductMaterial())
            .RuleFor(o => o.Description, f => f.Commerce.ProductDescription())
            .RuleFor(o => o.Uom, f => f.Random.AlphaNumeric(2))
            .RuleFor(o => o.AnvisaCode, f => f.Random.AlphaNumeric(9))
            .RuleFor(o => o.AnvisaDueDate, f => f.Date.Future(2).AsUtc())
            .RuleFor(o => o.SupplierCode, f => f.Random.AlphaNumeric(8))
            .RuleFor(o => o.Cst, f => f.Random.AlphaNumeric(3))
            .RuleFor(o => o.SusCode, f => f.Random.AlphaNumeric(7))
            .RuleFor(o => o.NcmCode, f => f.Random.AlphaNumeric(10));
    }

    public static Faker<ItemUpdateDto> GetFakerItemUpdateDto()
    {
        return new Faker<ItemUpdateDto>()
            .RuleFor(o => o.Code, f => f.Random.AlphaNumeric(8))
            .RuleFor(o => o.Name, f => f.Commerce.ProductName())
            .RuleFor(o => o.SalesName, f => f.Commerce.ProductMaterial())
            .RuleFor(o => o.Description, f => f.Commerce.ProductDescription())
            .RuleFor(o => o.Uom, f => f.Random.AlphaNumeric(2))
            .RuleFor(o => o.AnvisaCode, f => f.Random.AlphaNumeric(9))
            .RuleFor(o => o.AnvisaDueDate, f => f.Date.Future(2).AsUtc())
            .RuleFor(o => o.SupplierCode, f => f.Random.AlphaNumeric(8))
            .RuleFor(o => o.Cst, f => f.Random.AlphaNumeric(3))
            .RuleFor(o => o.SusCode, f => f.Random.AlphaNumeric(7))
            .RuleFor(o => o.NcmCode, f => f.Random.AlphaNumeric(10));
    }
}