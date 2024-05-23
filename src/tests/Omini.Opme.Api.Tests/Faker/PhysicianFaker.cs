using Bogus;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Business.Commands;

namespace Omini.Opme.Api.Tests;

public static class PhysicianFaker
{
    public static CreatePhysicianCommand GetFakePhysicianCreateDto()
    {
        var personName = PersonNameFaker.PersonName();

        return new Faker<CreatePhysicianCommand>()
            .RuleFor(o => o.FirstName, f => personName.FirstName)
            .RuleFor(o => o.LastName, f => personName.LastName)
            .RuleFor(o => o.MiddleName, f => personName.MiddleName)
            .RuleFor(o => o.Crm, f => f.Random.AlphaNumeric(5))
            .RuleFor(o => o.Cro, f => f.Random.AlphaNumeric(5))
            .RuleFor(o => o.Comments, f => f.Company.Bs());
    }
    public static UpdatePhysicianCommand GetFakePhysicianUpdateDto(Guid id)
    {
        var personName = PersonNameFaker.PersonName();

        var faker = new Faker<UpdatePhysicianCommand>()
            .RuleFor(o => o.FirstName, f => personName.FirstName)
            .RuleFor(o => o.LastName, f => personName.LastName)
            .RuleFor(o => o.MiddleName, f => personName.MiddleName)
            .RuleFor(o => o.Crm, f => f.Random.AlphaNumeric(5))
            .RuleFor(o => o.Cro, f => f.Random.AlphaNumeric(5))
            .RuleFor(o => o.Comments, f => f.Company.Bs()).Generate();

        faker.Id = id;

        return faker;
    }
}