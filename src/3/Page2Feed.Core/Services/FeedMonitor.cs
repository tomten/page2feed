using System;
using Page2Feed.Core.Services.Interfaces;

namespace Page2Feed.Core.Services
{

    public class FeedMonitor : IFeedMonitor
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