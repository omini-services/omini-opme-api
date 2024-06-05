using System.Linq.Expressions;
using Omini.Opme.Domain.Common;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Domain.Repositories;

public interface IRespositoryMasterEntity<TEntity> : IDisposable where TEntity : MasterEntity
{
    Task Add(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByCode(string code, CancellationToken cancellationToken = default);
    Task<PagedResult<TEntity>> GetAll(int currentPage = 1, int pageSize = 100, string? orderByField = null, SortDirection sortDirection = SortDirection.Asc, string? queryField = null, string? queryValue = null, CancellationToken cancellationToken = default);
    void Update(TEntity entity, CancellationToken cancellationToken = default);
    void Delete(TEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    IQueryable<TEntity> Filter(IQueryable<TEntity> query, string? queryField, string? queryValue);
}