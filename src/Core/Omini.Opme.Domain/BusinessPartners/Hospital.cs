using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.ValueObjects;
using Omini.Opme.Shared;

namespace Omini.Opme.Domain.BusinessPartners;

public sealed class Hospital : MasterEntity
{
    public new CompanyName Name { get; private set; }
    public string Cnpj { get; private set; }
    public string Comments { get; private set; }

    private Hospital() { }

    public Hospital(string code, CompanyName name, string cnpj, string comments)
    {
        SetData(code, name, cnpj, comments);
    }

    public Hospital SetData(string code, CompanyName name, string cnpj, string comments)
    {
        Code = code;
        Name = name;
        Cnpj = Formatters.FormatCnpj(cnpj);
        Comments = comments;
        return this;
    }

    public override string GetSearchableString()
    {
        return Name.LegalName;
    }
}