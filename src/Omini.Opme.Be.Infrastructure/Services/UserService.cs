using Microsoft.AspNetCore.Http;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Shared.Extensions;
using System.Security.Claims;

namespace Omini.Opme.Be.Infrastructure.Services;

internal class UserService : IUserService
{
    private readonly IHttpContextAccessor _accessor;

    public UserService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public Guid UserId => GetUserId();
    public string Name => _accessor.HttpContext.User.Identity.Name;

    public IEnumerable<Claim> GetClaimsIdentity()
    {
        return _accessor.HttpContext.User.Claims;
    }

    public string GetUserEmail()
    {
        return IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : "";
    }

    public Guid GetUserId()
    {
        return IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
    }

    public bool IsAuthenticated()
    {
        return _accessor.HttpContext.User.Identity.IsAuthenticated;
    }

    public bool IsInRole(string role)
    {
        throw new NotImplementedException();
    }
}