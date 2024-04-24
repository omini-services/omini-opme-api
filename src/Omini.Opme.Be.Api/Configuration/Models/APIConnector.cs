namespace Omini.Opme.Be.Api.Configuration.Models;

public class APIConnector
{
    protected string BasicAuthUsername { get; set; }
    protected string BasicAuthPassword { get; set; }
}

public class APIConnectors
{
    public APIConnector SignInSignUpExtension { get; set; }
}
