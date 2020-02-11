using System.Diagnostics.CodeAnalysis;

namespace Page2Feed.Auth.Google
{

    /// <summary>
    /// Open ID configuration.
    /// </summary>
    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Auto-generated code")]
    [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Auto-generated code")]
    public class OpenIdConfiguration
    {

        public string Issuer { get; set; }
        public string Authorization_Endpoint { get; set; }

        public string Token_Endpoint { get; set; }
        public string UserinfoEndpoint { get; set; }
        public string RevocationEndpoint { get; set; }
        public string JwksUri { get; set; }

        public string[] ResponseTypesSupported { get; set; }
        public string[] SubjectTypesSupported { get; set; }
        public string[] IdTokenSigningAlgValuesSupported { get; set; }
        public string[] ScopesSupported { get; set; }
        public string[] TokenEndpointAuthMethodsSupported { get; set; }
        public string[] ClaimsSupported { get; set; }
        public string[] CodeChallengeMethodsSupported { get; set; }

    }

}