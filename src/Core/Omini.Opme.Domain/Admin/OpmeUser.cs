using Omini.Opme.Domain.Entities;

namespace Omini.Opme.Domain.Admin;

public sealed class OpmeUser : SoftDeletable
{
    public string Email { get; set; }
}