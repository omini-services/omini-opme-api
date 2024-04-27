using System.Security.Claims;
using Omini.Opme.Be.Shared.Interfaces;

namespace BeforeSignUp;

public class ClaimsService : IClaimsService
{
    public Guid UserId => new Guid();

    public ClaimsPrincipal GetClaimsPrincipal()
    {
        throw new NotImplementedException();
    }
}