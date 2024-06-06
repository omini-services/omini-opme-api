using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.ValueObjects;

namespace Omini.Opme.Domain.Admin;

public sealed class InternalSpecialist : MasterEntity
{
    internal InternalSpecialist()
    {    
    }
    
    public InternalSpecialist(PersonName name, string telefone, string email)
    {
        Name = name;
        Telefone = telefone;
        Email = email;
    }

    public new PersonName Name { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
}