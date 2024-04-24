using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Shared.Entities;
using Omini.Opme.Be.Shared.Interfaces;

namespace Omini.Opme.Be.Infrastructure.Services;

internal class AuditableService : IAuditableService
{
    private readonly IClaimsProvider _claimsProvider;
    public AuditableService(IClaimsProvider claimsProvider)
    {
        _claimsProvider = claimsProvider;
    }
    public void SoftDelete<T>(T entity) where T : Auditable
    {
        entity.IsDeleted = true;
        entity.DeletedBy = _claimsProvider.GetUserId();
        entity.DeletedAt = DateTime.UtcNow;
    }
}