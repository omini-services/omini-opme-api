using Omini.Opme.Be.Domain;
using Omini.Opme.Domain.Entities;

namespace Omini.Opme.Domain.BusinessPartners;

public sealed class Physician : Auditable
{
    public PersonName Name { get; set; }
    public string Cro { get; set; }
    public string Crm { get; set; }
    public string Comments { get; set; }
}