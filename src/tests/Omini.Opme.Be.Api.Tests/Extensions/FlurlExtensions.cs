
using Flurl.Http;
using Omini.Opme.Be.Api.Tests.Authentication;

namespace Omini.Opme.Be.Api.Tests.Extensions;

public static class FlurlExtensions
{
    public static IFlurlRequest AsAuthenticated(this IFlurlRequest request, Func<string>? GetToken = null)
    {
        string bearer;
        if (GetToken is null)
        {
            bearer = new TestJwtToken().WithOpme(Guid.NewGuid()).Build();
        }
        else
        {
            bearer = GetToken();
        }

        return request.WithOAuthBearerToken(bearer);
    }
}