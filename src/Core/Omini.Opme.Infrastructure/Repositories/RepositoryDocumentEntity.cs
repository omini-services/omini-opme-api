using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure;

internal abstract class RepositoryDocumentEntity<TEntity> : IRespositoryDocumentEntity<TEntity> where TEntity : DocumentEntity
{
    protected readonly OpmeContext Db;
    protected readonly DbSet<TEntity> DbSet;

    protected RepositoryDocumentEntity(OpmeContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByNumber(long number, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().Where(p => p.Number == number).SingleOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(p => p.Id == id).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Shared.Entities.PagedResult<TEntity>> GetPagedResult(IQueryable<TEntity> query, int currentPage = 1, int pageSize = 100, CancellationToken cancellationToken = default)
    {
        if (currentPage < 1)
        {
            currentPage = 1;
        }

        if (pageSize == 0)
        {
            pageSize = int.MaxValue;
        }

        var paginatedQuery = new
        {
            TotalCount = query.Count(),
            Data = await query.Skip((currentPage - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync(cancellationToken)
        };

        return new Shared.Entities.PagedResult<TEntity>(paginatedQuery.Data, currentPage, pageSize, paginatedQuery.TotalCount);
    }

    public virtual async Task<Shared.Entities.PagedResult<TEntity>> GetAll(int currentPage = 1, int pageSize = 100, string? orderByField = null, SortDirection sortDirection = SortDirection.Asc, string? queryField = null, string? queryValue = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking();

        if (queryField is not null)
        {
            query.Where($"{queryField} = {queryValue}");
        }

        if (orderByField is not null)
        {
            var orderBy = sortDirection == SortDirection.Desc ? "DESC" : "ASC";
            query.OrderBy($"{orderByField} {orderBy}");
        }
        else
        {
            query.OrderBy(p => p.CreatedOn);
        }

        return await GetPagedResult(query, currentPage, pageSize, cancellationToken);
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