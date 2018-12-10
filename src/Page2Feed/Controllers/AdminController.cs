using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Page2Feed.Services.Interfaces;

namespace Page2Feed.Controllers
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
            ViewBag.Feeds = await _feedService.GetFeeds();
            return View();
        }

    }

}