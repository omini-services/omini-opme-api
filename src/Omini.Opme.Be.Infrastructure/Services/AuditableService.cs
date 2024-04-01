using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Infrastructure.Services;

internal class AuditableService : IAuditableService
{
    private readonly IUserService _userService;
    public AuditableService(IUserService userService)
    {
        _userService = userService;
    }
    public void SoftDelete<T>(T entity) where T : Auditable
    {
        entity.IsDeleted = true;
        entity.DeletedBy = _userService.GetUserId();
        entity.DeletedAt = DateTime.UtcNow;
    }
}