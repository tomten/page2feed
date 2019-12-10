namespace GoogleSignInTest.GoogleOidc
{

    /// <summary>
    /// Open ID configuration.
    /// </summary>
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