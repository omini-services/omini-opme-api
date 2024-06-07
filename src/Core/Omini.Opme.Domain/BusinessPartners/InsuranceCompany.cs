using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.ValueObjects;
using Omini.Opme.Shared;

namespace Omini.Opme.Domain.BusinessPartners;

public sealed class InsuranceCompany : MasterEntity
{
    public new CompanyName Name { get; set; }
    public string Cnpj { get; private set; }
    public string Comments { get; set; }

    private InsuranceCompany()
    {
    }

    public InsuranceCompany(CompanyName name, string cnpj, string comments)
    {
        SetData(name, cnpj, comments);
    }

    public InsuranceCompany SetData(CompanyName name, string cnpj, string comments)
    {
        Name = name;
        Cnpj = Formatters.FormatCnpj(cnpj);
        Comments = comments;
        return this;
    }
}