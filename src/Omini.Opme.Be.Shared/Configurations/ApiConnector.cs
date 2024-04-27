namespace Omini.Opme.Be.Shared;

public record ApiConnector {
    public SignInSignUpExtension SignInSignUpExtension { get; set; }
}

public record SignInSignUpExtension {
    public string BasicAuthUsername { get; set; }
    public string BasicAuthPassword { get; set; }
}