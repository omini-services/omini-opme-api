using Omini.Opme.Be.Domain;
using Omini.Opme.Domain.Entities;
using Omini.Opme.Shared;

namespace Omini.Opme.Domain.BusinessPartners;

public sealed class Hospital : Auditable
{
    public CompanyName Name { get; set; }
    public string Cnpj { get; private set; }
    public string Comments { get; set; }

    public Hospital WithCnpj(string cnpj)
    {
        Cnpj = Formatters.FormatCnpj(cnpj);
        return this;
    }
}