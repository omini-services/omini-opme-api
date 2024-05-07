namespace Omini.Opme.Be.Domain.Entities;

public abstract class Auditable : Entity
{
  public Auditable() { }

  public Guid CreatedBy { get; set; }
  public DateTime CreatedDate { get; set; }
  public Guid? LastModifiedBy { get; set; }
  public DateTime? LastModified { get; set; }
  public Guid? DeletedBy { get; set; }
  public DateTime? DeletedAt { get; set; }
  public bool IsDeleted { get; set; } = false;
}