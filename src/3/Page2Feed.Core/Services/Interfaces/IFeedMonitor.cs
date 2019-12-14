using System;

namespace Page2Feed.Core.Services.Interfaces
{

    public interface IFeedMonitor
    {

        DateTimeOffset GetNextFeedCheck();

        void SetNextFeedCheck(DateTimeOffset dateTimeOffset);

    }

}