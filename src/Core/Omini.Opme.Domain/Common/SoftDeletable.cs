namespace Omini.Opme.Domain.Entities;

public abstract class SoftDeletable : Auditable
{
  public Guid? DeletedBy { get; set; }
  public DateTime? DeletedOn { get; set; }
  public bool IsDeleted { get; set; } = false;
}