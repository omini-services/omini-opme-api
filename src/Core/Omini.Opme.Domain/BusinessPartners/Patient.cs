using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.ValueObjects;
using Omini.Opme.Shared;

namespace Omini.Opme.Domain.BusinessPartners;

public sealed class Patient : MasterEntity
{
    public new PersonName Name { get; set; }
    public string Cpf { get; private set; }
    public string Comments { get; set; }

    private Patient()
    {
    }

    public Patient(string code, PersonName name, string cpf, string comments)
    {
        SetData(code, name, cpf, comments);
    }

    public void SetData(string code, PersonName name, string cpf, string comments)
    {
        Code = code;
        Name = name;
        Cpf = Formatters.FormatCpf(cpf);
        Comments = comments;
    }
}