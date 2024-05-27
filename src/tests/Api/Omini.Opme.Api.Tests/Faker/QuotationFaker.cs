using Bogus;
using FluentAssertions.Extensions;
using Omini.Opme.Business.Commands;
using Omini.Opme.Api.Dtos;

namespace Omini.Opme.Api.Tests;

internal static class QuotationFaker
{
    internal static CreateQuotationCommand GetFakeQuotationCreateCommand(List<ResponseDto<ItemOutputDto>> itemOutputDtos)
    {
        var faker = new Faker();

        var quotationCreateCommand = new Faker<CreateQuotationCommand>()
            .RuleFor(o => o.DueDate, f => f.Date.Future().AsUtc())
            .Generate();

        foreach (var item in itemOutputDtos)
        {
            quotationCreateCommand.Items.Add(new CreateQuotationCommand.CreateQuotationItems()
            {
                ItemCode = item.Data.Code,
                Quantity = faker.Random.Number(1, 100),
                UnitPrice = Math.Round(faker.Random.Decimal(max: 1000), 2)
            });
        }

        return quotationCreateCommand;
    }
}