using Bogus;
using Bogus.DataSets;
using Omini.Opme.Domain.ValueObjects;

namespace Omini.Opme.Api.Tests;

public static class CompanyFaker
{
    public static CompanyName CompanyName()
    {
        var company = new Company();
        var companyName = company.CompanyName();
        var companySuffix = company.CompanySuffix();

        return new Faker<CompanyName>()
            .CustomInstantiator(f => new CompanyName($"{companySuffix} {companyName}", companyName));
    }
}