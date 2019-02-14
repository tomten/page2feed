using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Page2Feed.Core.Services.Interfaces;

namespace Page2Feed.Web.Controllers
{

    [ApiController]
    public class FeedController : Controller
    {

        private readonly IFeedService _feedService;
        private readonly ILogger _log;

        public FeedController(IFeedService feedService)
        {
            _feedService = feedService;
            _log = LogManager.GetLogger("Feeds");
        }

        [Authorize]
        [Route("~/createFeed")]
        [HttpPost]
        public async Task<ActionResult<string>> CreateFeed(
            [FromForm]string feedName,
            [FromForm]string feedGroupName,
            [FromForm]string feedUri
        )
        {
            await _feedService.CreateFeed(
                User.Identity.Name,
                feedName: feedName,
                feedGroupName: feedGroupName,
                feedUri: feedUri
            );
            TempData["Message"] = $"Created feed '{feedGroupName}:{feedName}'.";
            return RedirectToAction(
                "Index",
                "Admin"
            );
        }

        [Authorize]
        [Route("~/deleteFeed")]
        [HttpPost]
        public async Task<ActionResult<string>> DeleteFeed(
            [FromForm]string feedName,
            [FromForm]string feedGroupName
        )
        {
            await _feedService.DeleteFeed(
                userName: User.Identity.Name,
                feedGroupName: feedGroupName,
                feedName: feedName
            );
            TempData["Message"] = $"Deleted feed '{feedGroupName}:{feedName}'.";
            return RedirectToAction(
                "Index",
                "Admin"
            );
        }

        [Route("~/{feedGroupName}/{feedName}.xml")]
        [HttpGet]
        public async Task<IActionResult> Get(
            string feedGroupName,
            string feedName
        )
        {
            _log.Trace($"Got request for feed {feedGroupName}:{feedName}...");
            try
            {
                _log.Trace($"Getting feed {feedGroupName}:{feedName}...");
                var userName = (await _feedService.GetAllFeeds()).Single(f => f.Group == feedGroupName && f.Name == feedName).UserName;
                var feed = await
                    _feedService.GetFeed(
                        userName,
                        feedGroupName,
                        feedName
                    );
                _log.Trace($"Got feed.");
                var feedTitle = $"{feed.Group}: {feed.Name}";
                var feedDescription = feed.Name;
                var lastUpdatedTime = !feed.Entries.Any() ? new DateTimeOffset?() : feed.Entries.Max(e => e.Timestamp);
                var atom =
                    _feedService
                        .Atom(
                            feed,
                            feedTitle,
                            feedDescription,
                            lastUpdatedTime ?? DateTimeOffset.MinValue,
                            Request.GetDisplayUrl()
                        );  
                _log.Trace($"Returning content for feed {feedGroupName}:{feedName}...");
                return Content(
                    atom.XmlString(),
                    "application/atom+xml"
                );
            }
            catch (Exception exception)
            {
                _log.Error($"Error when getting feed {feedGroupName}:{feedName}: {exception}");
                throw; // ???
            }
        }

    }

}