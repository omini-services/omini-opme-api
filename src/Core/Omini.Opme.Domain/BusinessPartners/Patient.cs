using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.ValueObjects;
using Omini.Opme.Shared.Formatters;

namespace Omini.Opme.Domain.BusinessPartners;

public sealed class Patient : MasterEntity
{
    public new PersonName Name { get; set; }
    public string Cpf { get; private set; }
    public string Comments { get; set; }

    private Patient()
    {
    }

    public Patient(PersonName name, string cpf, string comments)
    {
        SetData(name, cpf, comments);
    }

    public void SetData(PersonName name, string cpf, string comments)
    {
        Name = name;
        Cpf = Formatters.FormatCpf(cpf);
        Comments = comments;
    }
}