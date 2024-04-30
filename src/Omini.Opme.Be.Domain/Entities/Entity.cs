using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Domain;

public abstract class Entity : Auditable
{
    public Guid Id { get; set; }
}