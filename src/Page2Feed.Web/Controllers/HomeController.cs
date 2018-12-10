using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Page2Feed.Web.Models;

namespace Page2Feed.Web.Controllers
{

    [ApiController]
    public class FeedController : ControllerBase
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
            return RedirectToAction(
                "Index",
                "Admin",
                new { message = $"Created feed '{feedGroupName}:{feedName}'." }
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
                var lastUpdatedTime = feed.Entries.Max(e => e.Timestamp).GetValueOrDefault();
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


    public class AdminController : Controller
        {

            private readonly IFeedService _feedService;

            public AdminController(IFeedService feedService)
            {
                _feedService = feedService;
            }

            [Route("~/")]
            public async Task<IActionResult> Index(
                string message = null
            )
            {
                ViewBag.Message = message;
                ViewBag.Feeds = await _feedService.GetFeeds();
                return View();
            }

        }


    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
