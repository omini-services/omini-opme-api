
using Flurl.Http;
using Omini.Opme.Api.Tests.Authentication;

namespace Omini.Opme.Api.Tests.Extensions;

public static class FlurlExtensions
{
    public static IFlurlRequest AsAuthenticated(this IFlurlRequest request, Func<string>? GetToken = null)
    {
        var rootUserGuid = new Guid("c8c5ce24-820f-41ba-8560-d7a282d80d29");
        string bearer;
        if (GetToken is null)
        {
            bearer = new TestJwtToken().WithOpme(rootUserGuid).Build();
        }
        else
        {
            bearer = GetToken();
        }

        return request.WithOAuthBearerToken(bearer);
    }
}