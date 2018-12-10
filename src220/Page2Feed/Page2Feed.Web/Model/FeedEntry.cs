using System;

namespace Page2Feed.Web.Model
{

    public class FeedEntry
    {

        public DateTimeOffset? Timestamp { get; set; }

        public string Body { get; set; }

        public string Id { get; set; }

    }

}