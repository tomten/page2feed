using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Page2Feed.Core.Services.Interfaces;
using Page2Feed.Web.Models;

namespace Page2Feed.Web.Controllers
{

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
            var feeds = await _feedService.GetFeeds();
            var adminIndexViewModel =
                new AdminIndexViewModel
                {
                    Feeds =
                        feeds.Select(
                            feed =>
                                new AdminFeedViewModel
                                {
                                    Name = feed.Name,
                                    Group = feed.Group,
                                    Entries = feed.Entries.Count,
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