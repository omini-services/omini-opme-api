using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Omini.Opme.Be.Domain;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Infrastructure.Contexts;

namespace Omini.Opme.Be.Infrastructure;

internal abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly OpmeContext Db;
    protected readonly DbSet<TEntity> DbSet;

    protected Repository(OpmeContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public virtual async Task<TEntity?> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<List<TEntity>> GetAll()
    {
        return await DbSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task Add(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public virtual void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public virtual void Delete(Guid id)
    {
        DbSet.Remove(new TEntity { Id = id });
    }

    public void Dispose()
    {
        Db?.Dispose();
    }
}