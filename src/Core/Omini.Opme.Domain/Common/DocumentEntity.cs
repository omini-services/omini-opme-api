namespace Omini.Opme.Domain.Common;

public abstract class DocumentEntity : IAuditable, ISoftDeletable
{
    public Guid Id { get; init; }
    public long Number { get; private set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
}