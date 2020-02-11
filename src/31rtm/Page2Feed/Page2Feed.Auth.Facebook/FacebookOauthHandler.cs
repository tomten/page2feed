using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Page2Feed.Core.Helpers;
using Page2Feed.Core.Util;

namespace Page2Feed.Auth.Facebook
{

    public class FacebookOauthHandler
    {

        private const string FacebookOauthDialogUri = "https://www.facebook.com/v5.0/dialog/oauth";
        private const string FacebookGraphApiOauthAccessTokenEndpoint = "https://graph.facebook.com/v5.0/oauth/access_token";
        private const string FacebookGraphApiMeEndpoint = "https://graph.facebook.com/me";

        private static Uri _redirectUri; // HACK
        private readonly HttpClient _facebookOauthHttpClient = new HttpClient();

        public Uri MakeFacebookSignInUri(
            string facebookClientId,
            Uri facebookOidcSignInResponseUri
            )
        {
            _redirectUri = facebookOidcSignInResponseUri;
            var facebookOauthTokenParameters = new
            {
                client_id = facebookClientId,
                redirect_uri = facebookOidcSignInResponseUri,
                state = "asdf", // TODO
                response_type = "code",
                scope = "email"
            };
            var facebookSignInUri =
                    FacebookOauthDialogUri.Uri()
                        .AddParameters(facebookOauthTokenParameters)
                ;
            return facebookSignInUri;
        }

        public async Task<string> HandleSignInResponse(
            string facebookAppId,
            string oauthCode,
            string facebookAppSecret
        )
        {
            var accessToken = await
                GetAccessTokenFromOauthCode(
                    facebookAppId,
                    oauthCode,
                    facebookAppSecret
                    );
            var userProfileInformation = await
                GetUserProfileInformationUsingAccessToken(
                    facebookAppSecret,
                    accessToken
                    );
            var email = userProfileInformation.Email;
            return email;
        }

        private async Task<Me> GetUserProfileInformationUsingAccessToken(string facebookAppSecret, string accessToken)
        {
            var accessTokenAppSecretHash = MakeAppSecretProof(facebookAppSecret, accessToken);
            var meUri = FacebookGraphApiMeEndpoint.Uri()
                .AddParameters(new
                {
                    fields = "email",
                    access_token = accessToken,
                    appsecret_proof = accessTokenAppSecretHash
                });
            var meUriResponse = await _facebookOauthHttpClient.GetAsync(meUri);
            var mee = meUriResponse.Content;
            var meee = await mee.ReadAsStringAsync();
            var me = JsonConvert.DeserializeObject<Me>(meee);
            return me;
        }

        private async Task<string> GetAccessTokenFromOauthCode(string facebookAppId, string oauthCode, string facebookAppSecret)
        {
            var accessTokenEndpoint = FacebookGraphApiOauthAccessTokenEndpoint.Uri();
            var accessTokenParameters = new TokenRequest
            {
                code = oauthCode,
                client_id = facebookAppId,
                client_secret = facebookAppSecret,
                redirect_uri = _redirectUri.ToString()
            };
            var accessTokenParametersContent = new FormUrlEncodedContent(accessTokenParameters);
            var accessTokenResponse = await _facebookOauthHttpClient
                .PostAsync(accessTokenEndpoint, accessTokenParametersContent).ConfigureAwait(false);
            var accessTokenResponseContent = accessTokenResponse.Content;
            var accessTokenResponseContentString = await accessTokenResponseContent.ReadAsStringAsync().ConfigureAwait(false);
            var accessTokenResponseModel = accessTokenResponseContentString.FromJson<TokenResponse>();
            if (accessTokenResponseModel.Error != null) throw new Exception(accessTokenResponseModel.Error.Message); // HACK
            var accessToken = accessTokenResponseModel.Access_Token;
            return accessToken;
        }

        public string MakeAppSecretProof(
            string facebookAppSecret,
            string accessToken
            )
        {
            var hash =
                facebookAppSecret
                    .ToUtf8Bytes()
                    .HmacSha256(
                        accessToken
                            .ToUtf8Bytes()
                        );
            var hex = hash.Hex();
            return hex;
        }

    }

}
