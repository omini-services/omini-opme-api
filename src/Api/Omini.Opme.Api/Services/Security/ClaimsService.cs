using System.Security.Claims;
using Omini.Opme.Shared.Extensions;
using Omini.Opme.Shared.Services.Security;

namespace Omini.Opme.Api.Services.Security;

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
        return IsAuthenticated() ? _accessor.HttpContext.User.GetEmail() : null;
    }

    public Guid? OpmeUserId => GetOpmeUserId();
    public string? Email => GetEmail();

    private Guid? GetOpmeUserId()
    {
        return IsAuthenticated() ? ClaimsPrincipal.GetOpmeUserId() : null;
    }

    private string? GetEmail()
    {
        return IsAuthenticated() ? ClaimsPrincipal.GetEmail() : null;
    }

    private bool IsAuthenticated()
    {
        return _accessor.HttpContext.User.Identity.IsAuthenticated;
    }
}
