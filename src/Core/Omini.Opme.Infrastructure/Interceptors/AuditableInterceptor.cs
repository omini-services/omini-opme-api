using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Omini.Opme.Domain.Entities;
using Omini.Opme.Domain.Exceptions;
using Omini.Opme.Shared.Services.Security;

namespace Omini.Opme.Infrastructure.Interceptors;

public sealed class AuditableInterceptor : SaveChangesInterceptor
{
    private readonly IClaimsService _claimsService;
    public AuditableInterceptor(IClaimsService claimsService)
    {
        _claimsService = claimsService;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(
                eventData, result, cancellationToken);
        }

        UpdateAdded(eventData);
        UpdateModified(eventData);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAdded(DbContextEventData eventData)
    {
        var opmeUserId = _claimsService.OpmeUserId;
        if (opmeUserId is null)
        {
            throw new InvalidUserException();
        }

        IEnumerable<EntityEntry<Auditable>> auditables =
                    eventData
                        .Context
                        .ChangeTracker
                        .Entries<Auditable>()
                        .Where(e => e.State == EntityState.Added);

        foreach (EntityEntry<Auditable> auditable in auditables)
        {
            auditable.Entity.CreatedBy = opmeUserId.Value;
            auditable.Entity.CreatedOn = DateTime.UtcNow;
        }
    }

    private void UpdateModified(DbContextEventData eventData)
    {
        var opmeUserId = _claimsService.OpmeUserId;
        if (opmeUserId is null)
        {
            throw new InvalidUserException();
        }

        IEnumerable<EntityEntry<Auditable>> auditables =
                    eventData
                        .Context
                        .ChangeTracker
                        .Entries<Auditable>()
                        .Where(e => e.State == EntityState.Modified);

        foreach (EntityEntry<Auditable> auditable in auditables)
        {
            auditable.Entity.UpdatedBy = opmeUserId.Value;
            auditable.Entity.UpdatedOn = DateTime.UtcNow;
        }
    }
}