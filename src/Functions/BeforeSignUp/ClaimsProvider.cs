using System.Security.Claims;
using Omini.Opme.Be.Shared.Interfaces;

namespace BeforeSignUp;

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