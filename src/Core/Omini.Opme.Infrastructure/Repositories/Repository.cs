using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Omini.Opme.Domain.Entities;
using Omini.Opme.Domain.Exceptions;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Infrastructure;

internal abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
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

    public async Task<PagedResult<TEntity>> GetPagedResult(IQueryable<TEntity> query, int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
        {
            pageNumber = 1;
        }

        if (pageSize == 0)
        {
            pageSize = int.MaxValue;
        }

        var paginatedQuery = new
        {
            TotalCount = query.Count(),
            Data = await query.Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync()
        };

        return new PagedResult<TEntity>(paginatedQuery.Data, pageNumber, pageSize, paginatedQuery.TotalCount);
    }

    public virtual async Task<PagedResult<TEntity>> GetAllPaginated(int pageNumber = default, int pageSize = default, CancellationToken cancellationToken = default)
    {
        return await GetPagedResult(DbSet.AsNoTracking(), pageNumber, pageSize);
    }

    public virtual async Task Add(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public virtual void Update(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
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