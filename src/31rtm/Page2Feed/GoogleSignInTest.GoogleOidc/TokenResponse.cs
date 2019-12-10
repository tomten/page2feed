namespace GoogleSignInTest.GoogleOidc
{

    /// <summary>
    /// Response from token endpoint.
    /// </summary>
    public class TokenResponse : GoogleOidcResponse
    {

        public string Id_Token { get; set; }

    }

}