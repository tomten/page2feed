namespace Page2Feed.Core.Model
{

    /// <summary>
    /// State for a feed.
    /// </summary>
    public class FeedState
    {

        /// <summary>
        /// The thumbprint of the text version of the current state of the feed source.
        /// </summary>
        public string ContentTextThumbprint { get; set; }

        /// <summary>
        /// The text version of the current state of the feed source.
        /// </summary>
        public string ContentText { get; set; }

    }

}