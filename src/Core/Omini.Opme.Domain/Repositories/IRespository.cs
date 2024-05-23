using System.Linq.Expressions;
using Omini.Opme.Domain.Entities;

namespace Omini.Opme.Domain.Repositories;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    Task Add(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetAll(CancellationToken cancellationToken = default);
    void Update(TEntity entity, CancellationToken cancellationToken = default);
    void Delete(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}