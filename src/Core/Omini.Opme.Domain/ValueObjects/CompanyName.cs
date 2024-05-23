
namespace Omini.Opme.Domain;

public class CompanyName : ValueObject
{
    public CompanyName(string legalName, string tradeName)
    {
        LegalName = legalName;
        TradeName = tradeName;
    }

    public string LegalName { get; init; }
    public string TradeName { get; init; }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return LegalName;
        yield return TradeName;
    }
}