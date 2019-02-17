using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Page2Feed.Core.Services.Interfaces;
using Page2Feed.Web.Data;
using Page2Feed.Web.Models;

namespace Page2Feed.Web.Controllers
{

    [Authorize]
    public class AdminController : Controller
    {

        private readonly IFeedService _feedService;
        private readonly IFeedMonitor _feedMonitor;
        private readonly ApplicationDbContext _applicationDbContext;


        public AdminController(
            IFeedService feedService,
            IFeedMonitor feedMonitor,
            ApplicationDbContext applicationDbContext
            )
        {
            _feedService = feedService;
            _feedMonitor = feedMonitor;
            _applicationDbContext = applicationDbContext;
        }

        [Authorize(Roles = "SuperAdmin")]
        [Route("~/superadmin")]
        public async Task<IActionResult> SuperAdmin()
        {
            var users = _applicationDbContext.Users.ToList();
            var superAdmin = new SuperAdminViewModel();
            superAdmin.Users = new List<SuperAdminViewModel.User>();
            foreach (var user in users)
            {
                var feeds = (await _feedService.GetFeedsAsync(user.UserName)).ToList();
                superAdmin.Users.Add(new SuperAdminViewModel.User
                {
                    UserName = user.UserName,
                    Feeds = feeds
                });
            }
            return View(superAdmin);
        }

        [Route("~/")]
        public async Task<IActionResult> Index(
            string message = null
        )
        {
            ViewBag.Message = message;
            var feeds = await _feedService.GetFeedsAsync(User.Identity.Name);
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
                                    LastUpdated =
                                        feed.Entries.Any()
                                            ? feed.Entries.Max(entry => entry.Timestamp)
                                            : new DateTimeOffset?(),
                                    Link = Url.Action(
                                        nameof(FeedController.Get),
                                        nameof(FeedController),
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