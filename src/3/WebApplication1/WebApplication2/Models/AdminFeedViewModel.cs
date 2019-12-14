using System;

namespace Page2Feed.Web.Models
{

    public class AdminFeedViewModel
    {

        public string Name { get; set; }

        public string Group { get; set; }

        public string Link { get; set; }

        public int Entries { get; set; }

        public DateTimeOffset? LastUpdated { get; set; }

    }

}