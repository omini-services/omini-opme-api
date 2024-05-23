using System.Security.Claims;

namespace Omini.Opme.Shared.Interfaces;

public interface IClaimsService
{
    ClaimsPrincipal ClaimsPrincipal { get; }
    Guid? OpmeUserId { get; }
}