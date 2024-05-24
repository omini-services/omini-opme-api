using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Omini.Opme.Domain.Entities;
using Omini.Opme.Domain.Exceptions;
using Omini.Opme.Shared.Services.Security;

namespace Omini.Opme.Infrastructure.Interceptors;

public sealed class SoftDeletableInterceptor : SaveChangesInterceptor
{
    private readonly IClaimsService _claimsService;
    public SoftDeletableInterceptor(IClaimsService claimsService)
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

        UpdateDeleted(eventData);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateDeleted(DbContextEventData eventData)
    {
        var opmeUserId = _claimsService.OpmeUserId;
        if (opmeUserId is null)
        {
            throw new InvalidUserException();
        }

        IEnumerable<EntityEntry<SoftDeletable>> auditables =
                    eventData
                        .Context
                        .ChangeTracker
                        .Entries<SoftDeletable>()
                        .Where(e => e.State == EntityState.Deleted);

        foreach (EntityEntry<SoftDeletable> auditable in auditables)
        {
            auditable.State = EntityState.Modified;
            auditable.Entity.DeletedBy = opmeUserId.Value;
            auditable.Entity.DeletedOn = DateTime.UtcNow;
            auditable.Entity.IsDeleted = true;
        }
    }
}