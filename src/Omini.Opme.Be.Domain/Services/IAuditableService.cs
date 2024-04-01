using System.Security.Claims;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Domain.Services;

public interface IAuditableService
{
    void SoftDelete<T>(T entity) where T : Auditable;
}