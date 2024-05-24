using Bogus;
using Bogus.Extensions.Brazil;
using Omini.Opme.Business.Commands;

namespace Omini.Opme.Api.Tests;

public static class InsuranceCompanyFaker
{
    public static CreateInsuranceCompanyCommand GetFakeInsuranceCompanyCreateCommand()
    {
        var companyName = CompanyFaker.CompanyName();

        return new Faker<CreateInsuranceCompanyCommand>()
            .RuleFor(o => o.LegalName, f => companyName.LegalName)
            .RuleFor(o => o.TradeName, f => companyName.TradeName)
            .RuleFor(o => o.Cnpj, f => f.Company.Cnpj())
            .RuleFor(o => o.Comments, f => f.Company.Bs());
    }

    public static UpdateInsuranceCompanyCommand GetFakeInsuranceCompanyUpdateCommand(Guid id)
    {
        var companyName = CompanyFaker.CompanyName();

        var faker = new Faker<UpdateInsuranceCompanyCommand>()
            .RuleFor(o => o.LegalName, f => companyName.LegalName)
            .RuleFor(o => o.TradeName, f => companyName.TradeName)
            .RuleFor(o => o.Cnpj, f => f.Company.Cnpj())
            .RuleFor(o => o.Comments, f => f.Company.Bs()).Generate();

        faker.Id = id;

        return faker;
    }
}