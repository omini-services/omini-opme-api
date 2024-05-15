using Bogus;
using Bogus.Extensions.Brazil;
using Omini.Opme.Be.Api.Dtos;

namespace Omini.Opme.Be.Api.Tests;

public static class PatientFaker
{
    public static PatientCreateDto GetFakePatient()
    {
        var personName = PersonNameFaker.PersonName();

        return new Faker<PatientCreateDto>()
            .RuleFor(o => o.FirstName, f => personName.FirstName)
            .RuleFor(o => o.LastName, f => personName.LastName)
            .RuleFor(o => o.MiddleName, f => personName.MiddleName)
            .RuleFor(o => o.Cpf, f => f.Person.Cpf())
            .RuleFor(o => o.Comments, f => f.Company.Bs());
    }
}