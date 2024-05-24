using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Omini.Opme.Domain.Entities;

namespace Omini.Opme.Infrastructure.Interceptors;

public sealed class EntityInterceptor : SaveChangesInterceptor
{
    public EntityInterceptor()
    {
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

        UpdateEntity(eventData);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntity(DbContextEventData eventData)
    {
        IEnumerable<EntityEntry<Entity>> auditables =
                    eventData
                        .Context
                        .ChangeTracker
                        .Entries<Entity>()
                        .Where(e => e.State == EntityState.Added);

        foreach (EntityEntry<Entity> auditable in auditables)
        {
            auditable.Entity.Id = Guid.NewGuid();
        }
    }
}