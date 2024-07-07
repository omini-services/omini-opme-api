using System.Security.Claims;

namespace Omini.Opme.Shared.Services.Security;

public interface IClaimsService
{
    ClaimsPrincipal ClaimsPrincipal { get; }
    Guid? OpmeUserId { get; }
    string? Email { get; }
}