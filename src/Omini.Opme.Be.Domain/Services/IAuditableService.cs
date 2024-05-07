using Omini.Opme.Be.Domain.Entities;

namespace Omini.Opme.Be.Domain.Services;

public interface IAuditableService
{
    void SoftDelete<T>(T entity) where T : Auditable;
}