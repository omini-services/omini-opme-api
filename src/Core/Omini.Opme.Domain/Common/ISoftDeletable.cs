namespace Omini.Opme.Domain.Common;

public interface ISoftDeletable
{
  public Guid? DeletedBy { get; set; }
  public DateTime? DeletedOn { get; set; }
  public bool IsDeleted { get; set; }
}