namespace Omini.Opme.Be.Shared.Entities;

public abstract class Auditable
{
  public Auditable() { }

  public Guid CreatedBy { get; private set; }
  public DateTime CreatedDate { get; private set; }
  public Guid? LastModifiedBy { get; private set; }
  public DateTime? LastModified { get; private set; }
  public Guid? DeletedBy { get; set; }
  public DateTime? DeletedAt { get; set; }
  public bool IsDeleted { get; set; } = false;
}