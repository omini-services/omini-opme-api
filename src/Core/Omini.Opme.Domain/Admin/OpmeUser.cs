using Omini.Opme.Domain.Common;

namespace Omini.Opme.Domain.Admin;

public sealed class OpmeUser : IAuditable, ISoftDeletable
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}