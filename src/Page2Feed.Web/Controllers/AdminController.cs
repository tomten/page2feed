using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Page2Feed.Core.Services.Interfaces;
using Page2Feed.Web.Models;
using Page2Feed.Web.Program;

namespace Page2Feed.Web.Controllers
{

    [Authorize]
    public class AdminController : Controller
    {

        private readonly IFeedService _feedService;
        private readonly IFeedMonitor _feedMonitor;

        public AdminController(
            IFeedService feedService,
            IFeedMonitor feedMonitor
            )
        {
            _feedService = feedService;
            _feedMonitor = feedMonitor;
        }

        [Route("~/")]
        public async Task<IActionResult> Index(
            string message = null
        )
        {
            ViewBag.Message = message;
            var feeds = await _feedService.GetFeeds(User.Identity.Name);
            var adminIndexViewModel =
                new AdminIndexViewModel
                {
                    NextFeedCheck = _feedMonitor.GetNextFeedCheck(),
                    Feeds =
                        feeds.Select(
                            feed =>
                                new AdminFeedViewModel
                                {
                                    Name = feed.Name,
                                    Group = feed.Group,
                                    Entries = feed.Entries.Count,
                                    LastUpdated = !feed.Entries.Any() ? new DateTimeOffset?() : feed.Entries.Max(f => f.Timestamp),
                                    Link = Url.Action(
                                        "Get",
                                        "Feed",
                                        new
                                        {
                                            FeedGroupName = feed.Group,
                                            FeedName = feed.Name,
                                        }
                                    )
                                }
                        )
                };
            return View(adminIndexViewModel);
        }

    }

}