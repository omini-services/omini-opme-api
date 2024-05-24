using System.Security.Claims;
using Microsoft.Extensions.Options;
using Omini.Opme.Shared.Extensions;
using Omini.Opme.Shared;
using Omini.Opme.Shared.Services.Security;
using Omini.Opme.Shared.Constants;

namespace BeforeSignUp;

public class ClaimsService : IClaimsService
{
    private readonly ApiConnector _apiConnector;

    public ClaimsService(IOptions<ApiConnector> apiConnectorOptions)
    {
        _apiConnector = apiConnectorOptions.Value;
    }

    public ClaimsPrincipal ClaimsPrincipal => GetClaimsPrincipal();

    public Guid? OpmeUserId => ClaimsPrincipal.GetOpmeUserId();

    public ClaimsPrincipal GetClaimsPrincipal()
    {
        var claimsIdentity = new ClaimsIdentity(
            [
                new Claim(OpmeKeyRegisteredClaimNames.OpmeUserId, _apiConnector.ClientId)
            ]
        );

        return new ClaimsPrincipal(claimsIdentity);
    }
}