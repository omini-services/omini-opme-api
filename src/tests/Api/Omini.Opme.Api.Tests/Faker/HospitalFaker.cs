using Bogus;
using Bogus.Extensions.Brazil;
using Omini.Opme.Business.Commands;

namespace Omini.Opme.Api.Tests;

public static class HospitalFaker
{
    public static CreateHospitalCommand GetFakeHospitalCreateCommand()
    {
        var companyName = CompanyFaker.CompanyName();

        return new Faker<CreateHospitalCommand>()
            .RuleFor(o => o.LegalName, f => companyName.LegalName)
            .RuleFor(o => o.TradeName, f => companyName.TradeName)
            .RuleFor(o => o.Cnpj, f => f.Company.Cnpj())
            .RuleFor(o => o.Comments, f => f.Company.Bs());
    }

    public static UpdateHospitalCommand GetFakeHospitalUpdateCommand(string code)
    {
        var companyName = CompanyFaker.CompanyName();

        var faker = new Faker<UpdateHospitalCommand>()
            .RuleFor(o => o.LegalName, f => companyName.LegalName)
            .RuleFor(o => o.TradeName, f => companyName.TradeName)
            .RuleFor(o => o.Cnpj, f => f.Company.Cnpj())
            .RuleFor(o => o.Comments, f => f.Company.Bs()).Generate();

        faker.Code = code;

        return faker;
    }
}