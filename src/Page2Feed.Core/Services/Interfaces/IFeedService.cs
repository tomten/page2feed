﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Page2Feed.Core.Model;
using Page2Feed.Core.Model.Atom;

namespace Page2Feed.Core.Services.Interfaces
{

    public interface IFeedService
    {

        Task<Feed> GetFeed(
            string feedGroupName,
            string feedName
            );

        Task<IEnumerable<Feed>> GetFeeds();

        Task<FeedStateEx> GetCurrentState(
            string latestSummary, // TODO
            Uri uri
            );

        Task SaveFeed(Feed feed);

        Task CreateFeed(
            string feedName,
            string feedGroupName,
            string feedUri
            );

        AtomFeed Atom(
            Feed feed,
            string feedTitle,
            string feedDescription,
            DateTimeOffset lastUpdatedTime,
            string feedHref
            );

        Task ProcessFeeds();

    }

}