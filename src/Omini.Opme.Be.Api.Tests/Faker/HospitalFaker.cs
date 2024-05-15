using Bogus;
using Bogus.Extensions.Brazil;
using Omini.Opme.Be.Api.Dtos;

namespace Omini.Opme.Be.Api.Tests;

public static class HospitalFaker
{
    public static HospitalCreateDto GetFakeHospital()
    {
        var companyName = CompanyFaker.CompanyName();

        return new Faker<HospitalCreateDto>()
            .RuleFor(o => o.LegalName, f => companyName.LegalName)
            .RuleFor(o => o.TradeName, f => companyName.TradeName)
            .RuleFor(o => o.Cnpj, f => f.Company.Cnpj())
            .RuleFor(o => o.Comments, f => f.Company.Bs());
    }
}