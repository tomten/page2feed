using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Page2Feed.Core.Model;
using Page2Feed.Core.Model.Atom;

namespace Page2Feed.Core.Services.Interfaces
{

    public interface IFeedService
    {

        Task<Feed> GetFeed(
            string userName,
            string feedGroupName,
            string feedName
            );

        Task<IEnumerable<Feed>> GetFeedsAsync(string userName);

        Task<IEnumerable<Feed>> GetAllFeeds();

        Task<FeedState> GetCurrentStateAsync(Uri uri);

        Task SaveFeed(Feed feed);

        Task CreateFeed(
            string userName,
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

        Task DeleteFeed(
            string userName,
            string feedName, 
            string feedGroupName
            );

    }

}