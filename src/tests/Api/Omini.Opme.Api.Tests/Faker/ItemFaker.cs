using Bogus;
using FluentAssertions.Extensions;
using Omini.Opme.Business.Commands;

namespace Omini.Opme.Api.Tests;

public static class ItemFaker
{
    public static Faker<CreateItemCommand> GetFakerItemCreateCommand()
    {
        return new Faker<CreateItemCommand>()
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

    public static Faker<UpdateItemCommand> GetFakerItemUpdateCommand()
    {
        return new Faker<UpdateItemCommand>()
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