using System.Security.Claims;

namespace Omini.Opme.Be.Domain.Services;

public interface IUserService
{
    Guid UserId { get; }
    string Name { get; }
    Guid GetUserId();
    string GetUserEmail();
    bool IsAuthenticated();
    bool IsInRole(string role);
    IEnumerable<Claim> GetClaimsIdentity();
}