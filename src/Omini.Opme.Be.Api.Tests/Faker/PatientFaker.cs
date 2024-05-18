using Bogus;
using Bogus.Extensions.Brazil;
using Omini.Opme.Be.Api.Dtos;

namespace Omini.Opme.Be.Api.Tests;

public static class PatientFaker
{
    public static PatientCreateDto GetFakePatientCreateDto()
    {
        var personName = PersonNameFaker.PersonName();

        return new Faker<PatientCreateDto>()
            .RuleFor(o => o.FirstName, f => personName.FirstName)
            .RuleFor(o => o.LastName, f => personName.LastName)
            .RuleFor(o => o.MiddleName, f => personName.MiddleName)
            .RuleFor(o => o.Cpf, f => f.Person.Cpf())
            .RuleFor(o => o.Comments, f => f.Company.Bs());
    }

    public static PatientUpdateDto GetFakePatientUpdateDto(Guid id)
    {
        var personName = PersonNameFaker.PersonName();

        var faker = new Faker<PatientUpdateDto>()
            .RuleFor(o => o.FirstName, f => personName.FirstName)
            .RuleFor(o => o.LastName, f => personName.LastName)
            .RuleFor(o => o.MiddleName, f => personName.MiddleName)
            .RuleFor(o => o.Cpf, f => f.Person.Cpf())
            .RuleFor(o => o.Comments, f => f.Company.Bs()).Generate();

        faker.Id = id;

        return faker;
    }
}