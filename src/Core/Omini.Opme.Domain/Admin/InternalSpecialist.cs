using Omini.Opme.Domain.Entities;

namespace Omini.Opme.Domain.Admin;

public sealed class InternalSpecialist : Auditable
{
    public PersonName Name { get; set; }
}