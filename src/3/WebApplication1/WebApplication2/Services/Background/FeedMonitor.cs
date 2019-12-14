using System;

namespace Page2Feed.Web.Services.Background
{

    public class FeedMonitor
    {

        private DateTimeOffset _nextFeedCheck;

        public DateTimeOffset GetNextFeedCheck()
        {
            return _nextFeedCheck;
        }

        public void SetNextFeedCheck(DateTimeOffset nextFeedCheck)
        {
            _nextFeedCheck = nextFeedCheck;
        }

    }

}