
namespace Omini.Opme.Be.Domain;

public class CompanyName : BaseValueObject
{
    public CompanyName(string legalName, string tradeName)
    {
        LegalName = legalName;
        TradeName = tradeName;
    }

    public string LegalName { get; init; }
    public string TradeName { get; init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return LegalName;
        yield return TradeName;
    }
}