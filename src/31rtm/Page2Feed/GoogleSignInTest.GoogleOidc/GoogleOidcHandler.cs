using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GoogleSignInTest.Core.Helpers;
using Newtonsoft.Json;

namespace GoogleSignInTest.GoogleOidc
{

    /// <summary>
    /// Handles the Google OIDC process.
    /// </summary>
    public sealed class GoogleOidcHandler : IDisposable
    {

        private static readonly Uri GoogleOidcDiscoDocUri = new Uri("https://accounts.google.com/.well-known/openid-configuration");
        private readonly HttpClient _googleOidcHttpClient = new HttpClient();
        private static string _redirectUri; // HACK

        public async Task<string> HandleSignInResponse(
            string googleClientId,
            string oidcResponseCode,
            string googleClientSecret
            )
        {
            var googleOidcTokenEndpointString = await GetGoogleOidcTokenEndpoint().ConfigureAwait(false);
            var googleOidcTokenEndpoint = new Uri(googleOidcTokenEndpointString);
            var googleOidcTokenParameters = new TokenRequest
            {
                Code = oidcResponseCode,
                Client_Id = googleClientId,
                Client_Secret = googleClientSecret,
                Redirect_Uri = _redirectUri,
                Grant_Type = "authorization_code"
            };
            var googleOidcTokenParametersContent = new FormUrlEncodedContent(googleOidcTokenParameters);
            var googleOidcTokenResponse = await _googleOidcHttpClient.PostAsync(googleOidcTokenEndpoint, googleOidcTokenParametersContent).ConfigureAwait(false);
            var googleOidcTokenResponseContent = googleOidcTokenResponse.Content;
            var googleOidcTokenResponseContentString = await googleOidcTokenResponseContent.ReadAsStringAsync().ConfigureAwait(false);
            var googleOidcTokenResponseContentModel = JsonConvert.DeserializeObject<TokenResponse>(googleOidcTokenResponseContentString);
            if (googleOidcTokenResponseContentModel.Error != null) throw new Exception(googleOidcTokenResponseContentModel.Error); // HACK
            var googleOidcJwtToken = googleOidcTokenResponseContentModel.Id_Token;
            var emailClaim = new JwtSecurityTokenHandler().ReadJwtToken(googleOidcJwtToken).Claims.Single(claim => claim.Type == "email");
            googleOidcTokenParametersContent.Dispose();
            return emailClaim.Value;
        }

        private async Task<string> GetGoogleOidcTokenEndpoint()
        {
            var googleOidcDiscoDoc = await GetGoogleOidcDiscoDoc().ConfigureAwait(false);
            return googleOidcDiscoDoc.Token_Endpoint; // "https://oauth2.googleapis.com/token";
        }

        private async Task<OpenIdConfiguration> GetGoogleOidcDiscoDoc()
        {
            var googleOidcDiscoDocResponse = await _googleOidcHttpClient.GetAsync(GoogleOidcHandler.GoogleOidcDiscoDocUri).ConfigureAwait(false);
            var googleOidcDiscoDocString = await googleOidcDiscoDocResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var googleOidcDiscoDoc = JsonConvert.DeserializeObject<OpenIdConfiguration>(googleOidcDiscoDocString);
            return googleOidcDiscoDoc;
        }

        public async Task<string> CreateGoogleSignInUri(string googleClientId, string signInResponseUri)
        {
            _redirectUri = signInResponseUri;
            var googleOidcParameters = new
            {
                client_id = googleClientId,
                response_type = "code",
                scope = "openid email",
                redirect_uri = _redirectUri,
                state = "asdf",
                nonce = "fdsa"
            };
            var googleOidcAuthorizationEndpoint = await GetGoogleOidcAuthorizationEndpoint().ConfigureAwait(false);
            var googleSignInUri =
                    new Uri(googleOidcAuthorizationEndpoint)
                        .AddParameters(googleOidcParameters)
                ;
            return googleSignInUri.ToString();
        }

        private async Task<string> GetGoogleOidcAuthorizationEndpoint()
        {
            var googleOidcDiscoDoc = await GetGoogleOidcDiscoDoc().ConfigureAwait(false);
            return googleOidcDiscoDoc.Authorization_Endpoint; // "https://accounts.google.com/o/oauth2/v2/auth";
        }

        public void Dispose()
        {
            _googleOidcHttpClient.Dispose();
        }

    }

}