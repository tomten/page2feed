using System.Diagnostics.CodeAnalysis;

namespace Page2Feed.Auth.Google
{

    /// <summary>
    /// Response from token endpoint.
    /// </summary>
    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Auto-generated code")]
    public class TokenResponse : GoogleOidcResponse
    {

        public string Id_Token { get; set; }

    }

}