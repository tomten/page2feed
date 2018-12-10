using System;
using System.Collections.Generic;

namespace Page2Feed.Model
{

    public class Feed
    {

        public int Version { get; set; }

        public FeedState StoredState { get; set; }

        public Uri Uri { get; set; }

        public string Group { get; set; }

        public string Name { get; set; }

        public List<FeedEntry> Entries { get; set; }

        public string Id { get; set; }

        public override string ToString()
        {
            return $"{Group} {Name} (#{Entries.Count}) {Uri}";
        }
    }

}