using Bogus;
using Omini.Opme.Be.Domain;

public static class PersonNameFaker
{
    public static PersonName PersonName()
    {
        var person = new Person();

        return new Faker<PersonName>()
            .CustomInstantiator(f => new PersonName(person.FirstName, person.LastName));
    }
}