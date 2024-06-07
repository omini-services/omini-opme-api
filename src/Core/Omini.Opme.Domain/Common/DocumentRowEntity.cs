namespace Omini.Opme.Domain.Common;

public abstract class DocumentRowEntity
{
    public Guid DocumentId { get; protected set; }
    public int LineId { get; protected set; }
    public int LineOrder { get; protected set; }
}