namespace Omini.Opme.Be.Domain.Entities;

public sealed class Patient : Auditable
{
    public PersonName Name { get; set; }
    public string Cpf { get; set; }
    public string Comments { get; set; }
}