using System.Security.Claims;
using Omini.Opme.Be.Shared.Interfaces;

namespace Omini.Opme.Be.Api.Security;

public class ClaimsProvider : IClaimsProvider
{
    public ClaimsPrincipal GetClaimsPrincipal()
    {
        throw new NotImplementedException();
    }

    public Guid GetUserId()
    {
        return new Guid();
    }
}