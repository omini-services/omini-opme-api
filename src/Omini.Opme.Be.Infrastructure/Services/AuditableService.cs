using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Shared.Interfaces;

namespace Omini.Opme.Be.Infrastructure.Services;

internal class AuditableService : IAuditableService
{
    private readonly IClaimsService _claimsService;
    public AuditableService(IClaimsService claimsService)
    {
        _claimsService = claimsService;
    }
    public void SoftDelete<T>(T entity) where T : Auditable
    {
        entity.IsDeleted = true;
        entity.DeletedBy = _claimsService.OpmeUserId;
        entity.DeletedAt = DateTime.UtcNow;
    }
}