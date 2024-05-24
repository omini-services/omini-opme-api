using System.Runtime.Serialization;
using Omini.Opme.Domain.Entities;
using Omini.Opme.Shared;

namespace Omini.Opme.Domain.BusinessPartners;

public sealed class Hospital : Auditable
{
    public CompanyName Name { get; private set; }
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