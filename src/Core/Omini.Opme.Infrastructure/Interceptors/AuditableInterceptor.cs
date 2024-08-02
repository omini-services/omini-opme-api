using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.Exceptions;
using Omini.Opme.Shared.Services.Security;

namespace Omini.Opme.Infrastructure.Interceptors;

internal sealed class AuditableInterceptor : SaveChangesInterceptor
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

        var auditables =
                    eventData
                        .Context!
                        .ChangeTracker.Entries()
                        .Where(e => typeof(IAuditable).IsAssignableFrom(e.Entity.GetType()) && e.State == EntityState.Added);

        foreach (var auditable in auditables)
        {
            var auditableEntity = auditable.Entity as IAuditable;
            auditableEntity!.CreatedBy = opmeUserId.Value;
            auditableEntity.CreatedOn = DateTime.UtcNow;
        }
    }

    private void UpdateModified(DbContextEventData eventData)
    {
        var opmeUserId = _claimsService.OpmeUserId;
        if (opmeUserId is null)
        {
            throw new InvalidUserException();
        }

        var auditables =
                    eventData
                        .Context!
                        .ChangeTracker.Entries()
                        .Where(e => typeof(IAuditable).IsAssignableFrom(e.Entity.GetType()) && e.State == EntityState.Modified);

        foreach (var auditable in auditables)
        {
            var auditableEntity = auditable.Entity as IAuditable;
            auditableEntity!.UpdatedBy = opmeUserId.Value;
            auditableEntity.UpdatedOn = DateTime.UtcNow;
        }
    }
}
