using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Page2Feed.Auth.Facebook;
using Page2Feed.Auth.Google;
using Page2Feed.Core.Helpers;
using Xunit;

namespace Page2Feed.Tests.Auth
{

    public class GoogleOidcTests
    {

        [Fact]
        public void Can_make_app_secret_proof()
        {

            var arrange = new FacebookOauthHandler();
            var act = arrange.MakeAppSecretProof("hej", "hopp");
            Assert.Equal(
                BitConverter.ToString(new HMACSHA256(Encoding.UTF8.GetBytes("hej")).ComputeHash(Encoding.UTF8.GetBytes("hopp"))).Replace("-", "").ToLowerInvariant(),
                act
                );

        }

        [Fact]
        public void Google_OIDC_token_parameters_can_auto_enumerate()
        {

            var dto = new Page2Feed.Auth.Google.TokenRequest { Code = "c" };
            var list = dto.ToList();
            Assert.Equal("c", list.Single(d => d.Key == "code").Value);

        }

        [Fact]
        public void URI_parameter_adding_makes_correct_URIs()
        {

            var u =
                new Uri("https://hej")
                .AddParameters(new { anka = "duck", banka = "bonk" });

            var actual = u.ToString();

            Assert.Equal("https://hej/?anka=duck&banka=bonk", actual);

        }

        [Fact]
        public void FromBase64String_needs_formal_padding()
        {

            var base64WithoutPadding =
                "eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiI4MTA2MzU2NzQ0MzUtZmhhc2pldTJsdG83YjIydXVkYThoOG00anFoaDNsM2kuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiI4MTA2MzU2NzQ0MzUtZmhhc2pldTJsdG83YjIydXVkYThoOG00anFoaDNsM2kuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJzdWIiOiIxMDMxODc5NzQ3MzI2ODc0MjkwNDQiLCJlbWFpbCI6InRvbXRlbkBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiYXRfaGFzaCI6InBKVHlGc1U2UXBPRExlajk2amg5aXciLCJub25jZSI6ImZkc2EiLCJpYXQiOjE1NzM5NDQwMjksImV4cCI6MTU3Mzk0NzYyOX0";
            string b = base64WithoutPadding;
            while (b.Length % 4 != 0) b += "=";

            var t = Convert.FromBase64String(b);

        }

        [Fact]
        public void JwtSecurityTokenHandler_extracts_claims_from_JWTs()
        {
            var jwtIdTokenExampleString = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImRiMDJhYjMwZTBiNzViOGVjZDRmODE2YmI5ZTE5NzhmNjI4NDk4OTQiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiI4MTA2MzU2NzQ0MzUtZmhhc2pldTJsdG83YjIydXVkYThoOG00anFoaDNsM2kuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiI4MTA2MzU2NzQ0MzUtZmhhc2pldTJsdG83YjIydXVkYThoOG00anFoaDNsM2kuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJzdWIiOiIxMDMxODc5NzQ3MzI2ODc0MjkwNDQiLCJlbWFpbCI6InRvbXRlbkBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiYXRfaGFzaCI6IjZVS3JHb0RpMEtlaXVybHZnQ0o2VmciLCJub25jZSI6ImZkc2EiLCJpYXQiOjE1NzQwMDk2MTMsImV4cCI6MTU3NDAxMzIxM30.KVqX1U1y_vl0Pz8xNA8rWVGuLJZXAlCUd2Xt_X5HxrjTxdSQeChFxnRL_GKya-M12lvi8X3_3fxo1wOToTXoIVzEL5t3vqJOl1_saqCwNvl7WQB3E6fvI0dPCdmVaqptcRHaRHvWMFcSZ7p4zzOXVO8uTTirVvNx5Ww4GTxCsYtghWhaihnwue--YI9HLgJXUXot6fYpSJC-uaEvh8Z6VtPtImBDfLP3Xayb2KZwPiGnGANrRhTzHLWbxhuQrkOBIHmiJWYWSk3cKVOob8Rxc7QUmFQ6NndSQWcAYGgU26Xnn4fGNCi1zx4M4m_mLuAjag0PrBSWE_AvlH-grQcT-g";
            var jwtidTokenExample = new JwtSecurityTokenHandler().ReadJwtToken(jwtIdTokenExampleString);
            var claims = jwtidTokenExample.Claims.ToList();
            Assert.Equal(
                "tomten@gmail.com",
                claims.Single(claim => claim.Type == "email").Value
                );

        }

    }

}
