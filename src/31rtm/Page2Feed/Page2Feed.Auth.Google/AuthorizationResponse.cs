namespace Page2Feed.Auth.Google
{

    /// <summary>
    /// Sent in callback from authorization endpoint.
    /// </summary>
    public class AuthorizationResponse : GoogleOidcResponse
    {

        public string Code { get; set; }

    }

}