using Bogus;
using Bogus.Extensions.Brazil;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Business.Commands;

namespace Omini.Opme.Api.Tests;

public static class HospitalFaker
{
    public static CreateHospitalCommand GetFakeHospitalCreateDto()
    {
        var companyName = CompanyFaker.CompanyName();

        return new Faker<CreateHospitalCommand>()
            .RuleFor(o => o.LegalName, f => companyName.LegalName)
            .RuleFor(o => o.TradeName, f => companyName.TradeName)
            .RuleFor(o => o.Cnpj, f => f.Company.Cnpj())
            .RuleFor(o => o.Comments, f => f.Company.Bs());
    }

    public static UpdateHospitalCommand GetFakeHospitalUpdateDto(Guid id)
    {
        var companyName = CompanyFaker.CompanyName();

        var faker = new Faker<UpdateHospitalCommand>()
            .RuleFor(o => o.LegalName, f => companyName.LegalName)
            .RuleFor(o => o.TradeName, f => companyName.TradeName)
            .RuleFor(o => o.Cnpj, f => f.Company.Cnpj())
            .RuleFor(o => o.Comments, f => f.Company.Bs()).Generate();

        faker.Id = id;

        return faker;
    }
}