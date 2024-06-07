namespace Omini.Opme.Domain.Common;

public abstract class MasterEntity : IAuditable
{
    public string Code { get; protected set; }
    public string Name { get; protected set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}