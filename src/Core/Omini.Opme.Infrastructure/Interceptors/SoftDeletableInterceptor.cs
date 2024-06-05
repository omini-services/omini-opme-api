using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Omini.Opme.Domain.Common;
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

        var softDeletables =
                    eventData
                        .Context
                        .ChangeTracker.Entries()
                        .Where(e => typeof(ISoftDeletable).IsAssignableFrom(e.Entity.GetType()) && e.State == EntityState.Deleted);

        foreach (var softDeletable in softDeletables)
        {
            var auditableEntity = softDeletable.Entity as ISoftDeletable;

            softDeletable.State = EntityState.Modified;
            auditableEntity.DeletedBy = opmeUserId.Value;
            auditableEntity.DeletedOn = DateTime.UtcNow;
            auditableEntity.IsDeleted = true;
        }
    }
}