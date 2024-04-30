namespace Omini.Opme.Be.Api.Configuration.Models;

internal class APIConnector
{
    protected string BasicAuthUsername { get; set; }
    protected string BasicAuthPassword { get; set; }
}

internal class APIConnectors
{
    public APIConnector SignInSignUpExtension { get; set; }
}
