using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.ValueObjects;

namespace Omini.Opme.Domain.BusinessPartners;

public sealed class Physician : MasterEntity
{
    public Physician(PersonName name, string cro, string crm, string comments)
    {
        SetData(name, cro, crm, comments);
    }

    private Physician() { }

    public new PersonName Name { get; set; }
    public string Cro { get; set; }
    public string Crm { get; set; }
    public string Comments { get; set; }

    public void SetData(PersonName name, string cro, string crm, string comments)
    {
        Name = name;
        Cro = cro;
        Crm = crm;
        Comments = comments;
    }
}