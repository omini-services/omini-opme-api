using Bogus;
using Omini.Opme.Be.Api.Dtos;

namespace Omini.Opme.Be.Api.Tests;

public static class PhysicianFaker
{
    public static PhysicianCreateDto GetFakePhysicianCreateDto()
    {
        var personName = PersonNameFaker.PersonName();

        return new Faker<PhysicianCreateDto>()
            .RuleFor(o => o.FirstName, f => personName.FirstName)
            .RuleFor(o => o.LastName, f => personName.LastName)
            .RuleFor(o => o.MiddleName, f => personName.MiddleName)
            .RuleFor(o => o.Crm, f => f.Random.AlphaNumeric(5))
            .RuleFor(o => o.Cro, f => f.Random.AlphaNumeric(5))
            .RuleFor(o => o.Comments, f => f.Company.Bs());
    }
    public static PhysicianUpdateDto GetFakePhysicianUpdateDto(Guid id)
    {
        var personName = PersonNameFaker.PersonName();

        var faker = new Faker<PhysicianUpdateDto>()
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