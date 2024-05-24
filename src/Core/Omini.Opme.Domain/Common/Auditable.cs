namespace Omini.Opme.Domain.Entities;

public abstract class Auditable : Entity
{
  public Guid CreatedBy { get; set; }
  public DateTime CreatedOn { get; set; }
  public Guid? UpdatedBy { get; set; }
  public DateTime? UpdatedOn { get; set; }
}
