﻿namespace Omini.Opme.Domain.Common;

public interface IAuditable
{
  public Guid CreatedBy { get; set; }
  public DateTime CreatedOn { get; set; }
  public Guid? UpdatedBy { get; set; }
  public DateTime? UpdatedOn { get; set; }
}
