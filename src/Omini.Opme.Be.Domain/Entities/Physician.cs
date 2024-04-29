using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Domain.Entities;

public class Physician : Auditable
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}