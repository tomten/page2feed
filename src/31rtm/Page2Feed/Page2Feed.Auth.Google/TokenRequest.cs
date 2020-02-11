using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Page2Feed.Auth.Google
{

    /// <summary>
    /// Parameters to token endpoint.
    /// </summary>
    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Auto-generated code")]
    public class TokenRequest : IEnumerable<KeyValuePair<string, string>>
    {

        public string Code { get; set; }

        public string Client_Id { get; set; }
        public string Client_Secret { get; set; }
        public string Redirect_Uri { get; set; }
        public string Grant_Type { get; set; }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(nameof(Code).ToLowerInvariant(), Code),
                new KeyValuePair<string, string>(nameof(Client_Id).ToLowerInvariant(), Client_Id),
                new KeyValuePair<string, string>(nameof(Client_Secret).ToLowerInvariant(), Client_Secret),
                new KeyValuePair<string, string>(nameof(Redirect_Uri).ToLowerInvariant(), Redirect_Uri),
                new KeyValuePair<string, string>(nameof(Grant_Type).ToLowerInvariant(), Grant_Type),
            }.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

    }

}