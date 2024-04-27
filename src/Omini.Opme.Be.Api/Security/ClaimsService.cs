using System.Security.Claims;
using Omini.Opme.Be.Shared.Interfaces;

namespace Omini.Opme.Be.Api.Security;

internal class ClaimsService : IClaimsService
{
    private readonly IHttpContextAccessor _accessor;
    
    public ClaimsService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }
    
    public ClaimsPrincipal GetClaimsPrincipal()
    {
        throw new NotImplementedException();
    }

    public Guid UserId => GetUserId();

    private Guid GetUserId()
    {
        return IsAuthenticated() ? new Guid() : Guid.Empty;// Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
    }

    private bool IsAuthenticated()
    {
        return _accessor.HttpContext.User.Identity.IsAuthenticated;
    }
}
