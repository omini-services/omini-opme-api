using Microsoft.EntityFrameworkCore;
using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure;

internal abstract class RepositoryDocumentRowEntity<TEntity> : IRespositoryDocumentRowEntity<TEntity> where TEntity : DocumentRowEntity
{
    protected readonly OpmeContext Db;
    protected readonly DbSet<TEntity> DbSet;

    protected RepositoryDocumentRowEntity(OpmeContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public virtual void Delete(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
    }

    public void Dispose()
    {
        Db?.Dispose();
    }
}