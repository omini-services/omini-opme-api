using System.Security.Claims;
using Omini.Opme.Be.Shared.Interfaces;
using Omini.Opme.Be.Shared.Extensions;

namespace Omini.Opme.Be.Api.Security;

internal class ClaimsService : IClaimsService
{
    private readonly IHttpContextAccessor _accessor;
    
    public ClaimsPrincipal GetClaimsPrincipal()
    {
        throw new NotImplementedException();
    }

    public Guid UserId => GetUserId();

    private Guid GetUserId()
    {
        return IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
    }

    private bool IsAuthenticated()
    {
        return _accessor.HttpContext.User.Identity.IsAuthenticated;
    }
}
