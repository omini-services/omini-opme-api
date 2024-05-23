using Bogus;
using FluentAssertions.Extensions;
using Omini.Opme.Business.Commands;
using Omini.Opme.Api.Dtos;

namespace Omini.Opme.Api.Tests;

public static class QuotationFaker
{
    public static CreateQuotationCommand GetFakeQuotationCreateDto(List<ResponseDto<ItemOutputDto>> itemOutputDtos)
    {
        var faker = new Faker();

        var quotationCreateDto = new Faker<CreateQuotationCommand>()
            .RuleFor(o => o.Number, f => f.Random.AlphaNumeric(5))
            .RuleFor(o => o.DueDate, f => f.Date.Future().AsUtc())
            .Generate();

        foreach (var item in itemOutputDtos)
        {
            quotationCreateDto.Items.Add(new QuotationCreateDto.QuotationCreateItemDto()
            {
                ItemCode = item.Data.Code,
                Quantity = faker.Random.Number(1, 100),
                UnitPrice = faker.Random.Double(max: 1000.0)
            });
        }

        return quotationCreateDto;
    }
}