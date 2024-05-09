
namespace Omini.Opme.Be.Domain;

public class PersonName : ValueObject
{
    public PersonName(string firstName, string middleName, string lastName)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
    }

    public string FirstName { get; init; }
    public string MiddleName { get; init; }
    public string LastName { get; init; }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return FirstName;
        yield return MiddleName;
        yield return LastName;
    }
}