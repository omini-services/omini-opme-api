using Bogus;
using Bogus.Extensions.Brazil;
using Omini.Opme.Business.Commands;

namespace Omini.Opme.Api.Tests;

public static class PatientFaker
{
    public static CreatePatientCommand GetFakePatientCreateCommand()
    {
        var personName = PersonNameFaker.PersonName();

        return new Faker<CreatePatientCommand>()
            .RuleFor(o => o.FirstName, f => personName.FirstName)
            .RuleFor(o => o.LastName, f => personName.LastName)
            .RuleFor(o => o.MiddleName, f => personName.MiddleName)
            .RuleFor(o => o.Cpf, f => f.Person.Cpf())
            .RuleFor(o => o.Comments, f => f.Company.Bs());
    }

    public static UpdatePatientCommand GetFakePatientUpdateCommand(Guid id)
    {
        var personName = PersonNameFaker.PersonName();

        var faker = new Faker<UpdatePatientCommand>()
            .RuleFor(o => o.FirstName, f => personName.FirstName)
            .RuleFor(o => o.LastName, f => personName.LastName)
            .RuleFor(o => o.MiddleName, f => personName.MiddleName)
            .RuleFor(o => o.Cpf, f => f.Person.Cpf())
            .RuleFor(o => o.Comments, f => f.Company.Bs()).Generate();

        faker.Id = id;

        return faker;
    }
}