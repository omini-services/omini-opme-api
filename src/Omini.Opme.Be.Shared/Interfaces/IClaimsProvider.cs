using System.Security.Claims;

namespace Omini.Opme.Be.Shared.Interfaces;

public interface IClaimsProvider{
    ClaimsPrincipal GetClaimsPrincipal();
    Guid GetUserId();
}