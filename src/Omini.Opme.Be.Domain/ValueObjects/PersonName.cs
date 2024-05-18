
namespace Omini.Opme.Be.Domain;

public class PersonName : ValueObject
{
    public PersonName(string firstName, string lastName, string? middleName = null)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }

    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? MiddleName { get; init; }

    public override IEnumerable<object?> GetAtomicValues()
    {
        yield return FirstName;
        yield return LastName;
        yield return MiddleName;
    }
}