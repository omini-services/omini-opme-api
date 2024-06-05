using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.ValueObjects;

namespace Omini.Opme.Domain.Admin;

public sealed class InternalSpecialist : MasterEntity
{
    public new PersonName Name { get; set; }
}