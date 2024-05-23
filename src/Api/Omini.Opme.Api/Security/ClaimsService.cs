using System.Security.Claims;
using Omini.Opme.Shared.Interfaces;
using Omini.Opme.Shared.Extensions;

namespace Omini.Opme.Api.Security;

internal class ClaimsService : IClaimsService
{
    private readonly IHttpContextAccessor _accessor;

    public ClaimsService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }
    public ClaimsPrincipal ClaimsPrincipal => _accessor.HttpContext.User;

    public string? GetUserEmail()
    {
        return IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : null;
    }

    public Guid? OpmeUserId => GetOpmeUserId();

    private Guid? GetOpmeUserId()
    {
        return IsAuthenticated() ? ClaimsPrincipal.GetOpmeUserId() : null;
    }

    private bool IsAuthenticated()
    {
        return _accessor.HttpContext.User.Identity.IsAuthenticated;
    }
}
