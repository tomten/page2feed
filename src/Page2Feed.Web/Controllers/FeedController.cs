using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Page2Feed.Core.Model.Atom;
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

        [Route("~/createFeed")]
        [HttpPost]
        public async Task<ActionResult<string>> CreateFeed(
            [FromForm]string feedName,
            [FromForm]string feedGroupName,
            [FromForm]string feedUri
        )
        {
            await _feedService.CreateFeed(
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

        [Route("~/deleteFeed")]
        [HttpPost]
        public async Task<ActionResult<string>> DeleteFeed(
            [FromForm]string feedName,
            [FromForm]string feedGroupName
        )
        {
            await _feedService.DeleteFeed(
                feedName: feedName,
                feedGroupName: feedGroupName
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
                var feed = await
                    _feedService.GetFeed(
                        feedGroupName,
                        feedName
                    );
                _log.Trace($"Got feed.");
                var feedTitle = $"{feed.Group}: {feed.Name}";
                var feedDescription = feed.Name;
                var lastUpdatedTime = feed.Entries.Max(e => e.Timestamp);
                var atom =
                    _feedService
                        .Atom(
                            feed,
                            feedTitle,
                            feedDescription,
                            lastUpdatedTime,
                            Request.GetDisplayUrl()
                        );
                var atomMemoryStream = new MemoryStream();
                var atomStringWriter = new StreamWriter(
                    atomMemoryStream,
                    Encoding.UTF8
                );
                new XmlSerializer(typeof(AtomFeed)).Serialize(
                    atomStringWriter,
                    atom
                );
                _log.Trace($"Returning content for feed {feedGroupName}:{feedName}...");
                return Content(
                    Encoding.UTF8.GetString(atomMemoryStream.GetBuffer()),
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