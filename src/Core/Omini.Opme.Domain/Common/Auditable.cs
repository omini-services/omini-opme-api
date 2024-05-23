namespace Omini.Opme.Domain.Entities;

public abstract class Auditable : Entity
{
  public Auditable() { }

  public Guid CreatedBy { get; set; }
  public DateTime CreatedOn { get; set; }
  public Guid? UpdatedBy { get; set; }
  public DateTime? UpdatedOn { get; set; }
}

public abstract class AuditableDeletable : Auditable
{
  public Guid? DeletedBy { get; set; }
  public DateTime? DeletedOn { get; set; }
  public bool IsDeleted { get; set; } = false;
}