using Omini.Opme.Domain;
using Omini.Opme.Domain.Entities;
using Omini.Opme.Shared;

namespace Omini.Opme.Domain.BusinessPartners;

public sealed class Patient : Auditable
{
    public PersonName Name { get; set; }
    public string Cpf { get; private set; }
    public string Comments { get; set; }

    public Patient WithCpf(string cpf){
        Cpf = Formatters.FormatCpf(cpf);
        return this;
    }
}