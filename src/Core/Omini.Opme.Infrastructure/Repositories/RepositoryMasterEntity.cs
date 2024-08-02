using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.Exceptions;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Infrastructure.Contexts;

namespace Omini.Opme.Infrastructure;

internal abstract class RepositoryMasterEntity<TEntity> : IRespositoryMasterEntity<TEntity> where TEntity : MasterEntity
{
    protected readonly OpmeContext Db;
    protected readonly DbSet<TEntity> DbSet;

    protected RepositoryMasterEntity(OpmeContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByCode(string code, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().Where(p => p.Code == code).SingleOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<Shared.Entities.PagedResult<TEntity>> GetAll(int currentPage = 1, int pageSize = 100, string? orderByField = null, SortDirection sortDirection = SortDirection.Asc, string? queryField = null, string? queryValue = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking();

        query = Filter(query, queryField, queryValue);

        if (orderByField is not null)
        {
            var orderBy = sortDirection == SortDirection.Desc ? "DESC" : "ASC";
            query = query.OrderBy($"{orderByField} {orderBy}");
        }
        else
        {
            query = query.OrderBy(p => p.CreatedOn);
        }

        return await GetPagedResult(query, currentPage, pageSize, cancellationToken);
    }

    public virtual async Task Create(TEntity entity, CancellationToken cancellationToken = default)
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

    private async Task<Shared.Entities.PagedResult<TEntity>> GetPagedResult(IQueryable<TEntity> query, int currentPage = 1, int pageSize = 100, CancellationToken cancellationToken = default)
    {
        if (currentPage < 1)
        {
            currentPage = 1;
        }

        if (pageSize == 0)
        {
            pageSize = 100;
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

    public virtual IQueryable<TEntity> Filter(IQueryable<TEntity> query, string? queryField, string? queryValue)
    {
        return query;
    }

    public void Dispose()
    {
        Db?.Dispose();
    }
}