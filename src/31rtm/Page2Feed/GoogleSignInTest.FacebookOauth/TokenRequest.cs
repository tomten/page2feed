using System.Collections;
using System.Collections.Generic;

namespace GoogleSignInTest.FacebookOauth
{
    public class TokenRequest : IEnumerable<KeyValuePair<string, string>>
    {

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(nameof(code).ToLowerInvariant(), code),
                new KeyValuePair<string, string>(nameof(client_id).ToLowerInvariant(), client_id),
                new KeyValuePair<string, string>(nameof(client_secret).ToLowerInvariant(), client_secret),
                new KeyValuePair<string, string>(nameof(redirect_uri).ToLowerInvariant(), redirect_uri),
            }.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        public string code { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string redirect_uri { get; set; }
    }
}