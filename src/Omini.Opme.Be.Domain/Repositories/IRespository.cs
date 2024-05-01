using System.Linq.Expressions;

namespace Omini.Opme.Be.Domain.Repositories;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    Task Add(TEntity entity);
    Task<TEntity?> GetById(Guid id);
    Task<List<TEntity>> GetAll();
    void Update(TEntity entity);
    void Delete(Guid id);
    Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate);
}