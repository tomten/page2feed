namespace Page2Feed.Auth.Facebook
{
    public class TokenResponse
    {
        public string Access_Token { get; set; }
        public string Token_Type { get; set; }
        public uint Expires_In { get; set; }
        public TokenResponseError Error { get; set; }
    }
}