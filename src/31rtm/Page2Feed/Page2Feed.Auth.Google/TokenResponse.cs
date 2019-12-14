namespace Page2Feed.Auth.Google
{

    /// <summary>
    /// Response from token endpoint.
    /// </summary>
    public class TokenResponse : GoogleOidcResponse
    {

        public string Id_Token { get; set; }

    }

}