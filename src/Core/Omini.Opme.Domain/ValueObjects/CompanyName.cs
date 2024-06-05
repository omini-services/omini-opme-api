namespace Omini.Opme.Domain.ValueObjects;

public class CompanyName : ValueObject
{
    public CompanyName(string legalName, string tradeName)
    {
        LegalName = legalName;
        TradeName = tradeName;
    }

    public string LegalName { get; set; }
    public string TradeName { get; set; }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return LegalName;
        yield return TradeName;
    }
}