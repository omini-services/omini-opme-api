using Omini.Opme.Domain.Entities;
using Omini.Opme.Shared;

namespace Omini.Opme.Domain.BusinessPartners;

public sealed class Patient : Auditable
{
    public PersonName Name { get; set; }
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