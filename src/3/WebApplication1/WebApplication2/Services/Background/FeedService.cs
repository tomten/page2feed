using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using Page2Feed.Core;
using Page2Feed.Core.Model;
using Page2Feed.Core.Model.Atom;
using Page2Feed.Web.App;

namespace Page2Feed.Web.Services.Background
{

    public class FeedService
    {

        private readonly FeedRepository _feedRepository;
        private readonly WebRepository _webRepository;
        private readonly Html2TextConverter _html2TextConverter;
        private readonly ILogger _log;

        public FeedService(
            FeedRepository feedRepository,
            WebRepository webRepository,
            Html2TextConverter html2TextConverter
        )
        {
            _feedRepository = feedRepository;
            _webRepository = webRepository;
            _html2TextConverter = html2TextConverter;
            _log = LogManager.GetLogger("Feeds");
        }

        public async Task<Feed> GetFeed(
            string userName,
            string feedGroupName,
            string feedName
        )
        {
            var feedId = await
                _feedRepository
                    .GetFeedId(
                        userName,
                        feedGroupName,
                        feedName
                    );
            var feed = await
                _feedRepository
                    .Get(feedId);
            return feed;
        }

        public async Task CreateFeed(
            string userName,
            string feedName,
            string feedGroupName,
            string feedUri
        )
        {
            var feed = new Feed
            {
                UserName = userName,
                Name = feedName,
                Group = feedGroupName,
                Entries = new List<FeedEntry>(),
                StoredState = new FeedState(),
                Uri = new Uri(feedUri),
                Version = 3,
                Id = Guid.NewGuid().ToString("D")
            };
            await SaveFeed(feed);
        }

        public AtomFeed Atom(
            Feed feed,
            string feedTitle,
            string feedDescription,
            DateTimeOffset lastUpdatedTime,
            string displayUrl
        )
        {
            var atom = new AtomFeed
            {
                id = feed.Id,
                title = feedTitle,
                subtitle = feedDescription,
                updated = lastUpdatedTime.ToString("O"),
                link = new[]
                {
                    new feedLink
                    {
                        rel = "self",
                        href = displayUrl                    }
                },
                entry = feed.Entries.Select(e => new feedEntry
                {
                    updated = e.Timestamp.ToString("O"),
                    title = $"{e.Body.PadRight(30).Substring(0, 30)}...",
                    id = e.Id,
                    link = new[] { new feedEntryLink { href = feed.Uri.ToString() } },
                    content = new feedEntryContent { type = "xhtml", div = e.Body },
                    summary = $"{e.Body.PadRight(30).Substring(0, 30)}...",
                    author = new feedEntryAuthor { email = "test@test.test", name = "test" }
                }).ToArray()
            };
            return atom;
        }

        public async Task<IEnumerable<Feed>> GetFeedsAsync(string userName)
        {
            return (await _feedRepository.GetAll()).Where(feed => feed.UserName == userName);
        }

        public async Task<IEnumerable<Feed>> GetAllFeeds()
        {
            return await _feedRepository.GetAll();
        }

        public string MakeThumbprint(string s)
        {
            return s.Md5Hex();
        }

        public async Task<FeedState> GetCurrentStateAsync(Uri uri)
        {
            var contentsCurrentHtml = await _webRepository.GetContents(uri);
            var contentsCurrentText = await _html2TextConverter.Html2TextAsync(contentsCurrentHtml);
            var contentThumbprint = MakeThumbprint(contentsCurrentText);
            var feedState =
                new FeedState
                {
                    ContentTextThumbprint = contentThumbprint,
                    ContentText = contentsCurrentText
                };
            return feedState;
        }

        public string MakeSummary(
            string contentsOld,
            string contents
        )
        {
            var contentsWithSameStartRemoved =
                TrimSameStart(
                    (contentsOld ?? "").Trim(),
                    contents.Trim()
                );
            return contentsWithSameStartRemoved;
        }

        public async Task ProcessFeeds()
        {
            _log.Trace("Getting feeds...");
            var feeds = (await GetAllFeeds()).ToList();
            _log.Trace($"Done getting feeds. {feeds.Count} feeds gotten ({string.Join(",", feeds.Select(f => f.Name))}).");
            foreach (var feed in feeds)
            {
                _log.Trace($"Processing feed {feed.Group}:{feed.Name}...");
                try
                {
                    var currentStateThumbprint = feed.StoredState.ContentTextThumbprint;
                    _log.Trace($"Getting current state for feed {feed.Name}...");
                    var newState = await GetCurrentStateAsync(feed.Uri);
                    _log.Trace($"Got current state for feed {feed.Name}: {newState.ContentTextThumbprint}.");
                    if (newState.ContentTextThumbprint != feed.StoredState.ContentTextThumbprint)
                    {
                        _log.Info($"State has changed for feed {feed.Group}:{feed.Name}; updating feed...");
                        _log.Trace($"Inserting entry into feed {feed.Name}...");
                        var newSummary =
                            MakeSummary(
                                feed.StoredState.ContentText,
                                newState.ContentText
                            );
                        feed.Entries.Insert(
                            0,
                            new FeedEntry
                            {
                                Timestamp = DateTimeOffset.Now,
                                Body = newSummary,
                                Id = Guid.NewGuid().ToString("D")
                            }
                        );
                        _log.Trace($"Done inserting entry into feed {feed.Name}.");
                        feed.StoredState.ContentTextThumbprint = newState.ContentTextThumbprint;
                        feed.StoredState.ContentText = newState.ContentText;
                        _log.Info($"Saving feed {feed.Name}...");
                        await SaveFeed(feed);
                        _log.Trace($"Done saving feed {feed.Name}.");
                        _log.Trace($"Done updating feed {feed.Name}...");
                    }

                    _log.Trace($"Done processing feed {feed.Group}:{feed.Name}.");
                }
                catch (Exception exception)
                {
                    _log.Error($"Error when processing feed {feed.Group}:{feed.Name}: {exception}");
                }
            }
        }

        public async Task DeleteFeed(
            string userName,
            string feedGroupName,
            string feedName
        )
        {
            var feedId = await
                _feedRepository
                    .GetFeedId(
                        userName,
                        feedGroupName,
                        feedName
                    );
            await _feedRepository.Delete(feedId);
        }

        public async Task SaveFeed(Feed feed)
        {
            await _feedRepository.Store(feed);
        }

        public string TrimSameStart(string s1, string s2)
        {
            if (s1 == s2)
                return "";

            var currentStringPosition = 0;
            while (
                currentStringPosition < s1.Length &&
                currentStringPosition < s2.Length &&
                s1[currentStringPosition] == s2[currentStringPosition]
            )
            {
                currentStringPosition++;
            }

            if (s2.Length == currentStringPosition)
                currentStringPosition--;

            while (
                currentStringPosition > 0 &&
                char.IsLetterOrDigit(s2[currentStringPosition])
            )
            {
                currentStringPosition--;
            }
            return s2.Substring(
                currentStringPosition + (
                    char.IsLetterOrDigit(s2[currentStringPosition])
                        ? 0
                        : 1
                ));
        }

    }

}