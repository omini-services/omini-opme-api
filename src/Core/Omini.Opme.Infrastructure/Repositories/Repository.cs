using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Omini.Opme.Domain.Entities;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure;

internal abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly OpmeContext Db;
    protected readonly DbSet<TEntity> DbSet;

    protected Repository(OpmeContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().Where(p => p.Id == id).SingleOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetAll(CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task Add(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public virtual void Update(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
    }

    public virtual void Delete(Guid id, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(new TEntity { Id = id });
    }

    public void Dispose()
    {
        Db?.Dispose();
    }
}