using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.ValueObjects;
using Omini.Opme.Shared.Formatters;

namespace Omini.Opme.Domain.BusinessPartners;

public sealed class Hospital : MasterEntity
{
    public new CompanyName Name { get; private set; }
    public string Cnpj { get; private set; }
    public string Comments { get; private set; }

    private Hospital() { }

    public Hospital(CompanyName name, string cnpj, string comments)
    {
        SetData(name, cnpj, comments);
    }

    public Hospital SetData(CompanyName name, string cnpj, string comments)
    {
        Name = name;
        Cnpj = Formatters.FormatCnpj(cnpj);
        Comments = comments;
        return this;
    }
}