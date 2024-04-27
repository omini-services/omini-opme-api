using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Shared.Entities;
using Omini.Opme.Be.Shared.Interfaces;

namespace Omini.Opme.Be.Infrastructure.Services;

internal class AuditableService : IAuditableService
{
    private readonly IClaimsService _claimsProvider;
    public AuditableService(IClaimsService claimsProvider)
    {
        _claimsProvider = claimsProvider;
    }
    public void SoftDelete<T>(T entity) where T : Auditable
    {
        entity.IsDeleted = true;
        entity.DeletedBy = _claimsProvider.UserId;
        entity.DeletedAt = DateTime.UtcNow;
    }
}