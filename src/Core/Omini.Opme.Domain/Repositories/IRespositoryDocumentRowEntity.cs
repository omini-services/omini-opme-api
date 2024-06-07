using Omini.Opme.Domain.Common;

namespace Omini.Opme.Domain.Repositories;

public interface IRespositoryDocumentRowEntity<TEntity> : IDisposable where TEntity : DocumentRowEntity
{
    void Delete(TEntity entity, CancellationToken cancellationToken = default);
}