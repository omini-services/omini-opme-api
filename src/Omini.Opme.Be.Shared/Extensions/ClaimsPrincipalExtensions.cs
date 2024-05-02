using System.Security.Claims;

namespace Omini.Opme.Be.Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentException("Claim userId not found", nameof(principal));
        }

        var claim = principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier");
        return claim?.Value;
    }

    public static string? GetUserEmail(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentException("Claim userEmail not found", nameof(principal));
        }

        var claim = principal.FindFirst(ClaimTypes.Email);
        return claim?.Value;
    }
}
