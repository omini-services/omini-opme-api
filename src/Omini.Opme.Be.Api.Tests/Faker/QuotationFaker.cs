using Bogus;
using Omini.Opme.Be.Api.Dtos;
using FluentAssertions.Extensions;

namespace Omini.Opme.Be.Api.Tests;

public static class QuotationFaker
{
    public static QuotationCreateDto GetFakeQuotationCreateDto(List<ResponseDto<ItemOutputDto>> itemOutputDtos)
    {
        var faker = new Faker();

        var quotationCreateDto = new Faker<QuotationCreateDto>()
            .RuleFor(o => o.Number, f => f.Random.AlphaNumeric(5))
            .RuleFor(o => o.DueDate, f => f.Date.Future().AsUtc())
            .Generate();

        foreach (var item in itemOutputDtos)
        {
            quotationCreateDto.Items.Add(new QuotationCreateDto.QuotationCreateItemDto()
            {
                ItemId = item.Data.Id,
                ItemCode = item.Data.Code,
                AnvisaCode = item.Data.AnvisaCode,
                AnvisaDueDate = item.Data.AnvisaDueDate,
                Quantity = faker.Random.Number(1, 100),
                UnitPrice = faker.Random.Double(max: 1000.0)
            });
        }

        return quotationCreateDto;
    }
}