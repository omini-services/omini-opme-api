namespace Omini.Opme.Be.Domain.Entities;

public class Physician : Entity
{
    public PersonName Name { get; set; }
    public string Cro { get; set; }
    public string Crm { get; set; }
    public string Comments { get; set; }
}