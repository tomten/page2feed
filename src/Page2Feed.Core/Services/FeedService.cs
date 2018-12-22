using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using NLog;
using Page2Feed.Core.Model;
using Page2Feed.Core.Model.Atom;
using Page2Feed.Core.Services.Interfaces;

namespace Page2Feed.Core.Services
{
    public class FeedService : IFeedService
    {

        private readonly IFeedRepository _feedRepository;
        private readonly IWebRepository _webRepository;
        private readonly IHtml2TextConverter _html2TextConverter;
        private readonly ILogger _log;

        public FeedService(
            IFeedRepository feedRepository,
            IWebRepository webRepository,
            IHtml2TextConverter html2TextConverter
            )
        {
            _feedRepository = feedRepository;
            _webRepository = webRepository;
            _html2TextConverter = html2TextConverter;
            _log = LogManager.GetLogger("Feeds");
        }

        public async Task<Feed> GetFeed(
            string feedGroupName,
            string feedName
            )
        {
            var feed = await
                _feedRepository
                    .Get(
                        feedGroupName,
                        feedName
                    );
            return feed;
        }

        public async Task CreateFeed(
            string feedName,
            string feedGroupName,
            string feedUri
            )
        {
            var feed = new Feed
            {
                Name = feedName,
                Group = feedGroupName,
                Entries = new List<FeedEntry>(),
                StoredState = new FeedState(),
                Uri = new Uri(feedUri),
                Version = 2,
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

        public async Task<IEnumerable<Feed>> GetFeeds()
        {
            return await _feedRepository.GetAll();
        }

        public string MakeThumbprint(string s)
        {
            return s.Md5Hex();
        }

        public async Task<FeedStateEx> GetCurrentStateAsync(
            string contentsOldText,
            Uri uri
            )
        {
            var contentsCurrentHtml = await _webRepository.GetContents(uri);
            var contentsCurrentText = await _html2TextConverter.Html2TextAsync(contentsCurrentHtml);
            var contentSummary = await
                MakeSummary(
                    contentsOldText,
                    contentsCurrentText
                );
            var thumbprint = MakeThumbprint(contentSummary);
            var feedState =
                new FeedStateEx
                {
                    ContentSummaryThumbprint = thumbprint,
                    ContentSummary = contentSummary
                };
            return feedState;
        }

        public async Task<string> MakeSummary(
            string contentsOld,
            string contents
            )
        {
            var contentsTrimmed =
                TrimSameStart(
                    contentsOld.Trim(),
                    contents.Trim()
                );
            return contentsTrimmed;
        }

        public async Task ProcessFeeds()
        {
            _log.Trace("Getting feeds...");
            var feeds = (await GetFeeds()).ToList();
            _log.Trace($"Done getting feeds. {feeds.Count} feeds gotten ({string.Join(",", feeds.Select(f => f.Name))}).");
            foreach (var feed in feeds)
            {
                _log.Trace($"Processing feed {feed.Group}:{feed.Name}...");
                try
                {
                    var storedContentSummaryThumbprint = feed.StoredState.ContentSummaryThumbprint;
                    var storedContentSummary = feed.Entries.FirstOrDefault()?.Body;
                    _log.Trace($"Getting current state for feed {feed.Name}...");
                    var newState = await GetCurrentStateAsync(storedContentSummary, feed.Uri);
                    _log.Trace($"Got current state for feed {feed.Name}: {newState.ContentSummaryThumbprint}.");
                    if (newState.ContentSummaryThumbprint != storedContentSummaryThumbprint)
                    {
                        _log.Info($"State has changed for feed {feed.Group}:{feed.Name}; updating feed...");
                        _log.Trace($"Inserting entry into feed {feed.Name}...");
                        feed.Entries.Insert(
                            0,
                            new FeedEntry
                            {
                                Timestamp = DateTimeOffset.Now,
                                Body = newState.ContentSummary,
                                Id = Guid.NewGuid().ToString("D")
                            }
                        );
                        _log.Trace($"Done inserting entry into feed {feed.Name}.");
                        feed.StoredState.ContentSummaryThumbprint = newState.ContentSummaryThumbprint;
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

        public async Task DeleteFeed(string feedGroupName, string feedName)
        {
            await _feedRepository.Delete(
                feedGroupName,
                feedName
                );
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