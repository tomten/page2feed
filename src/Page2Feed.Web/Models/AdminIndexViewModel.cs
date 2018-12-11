using System.Collections.Generic;

namespace Page2Feed.Web.Models
{
    public class AdminIndexViewModel
    {
        public IEnumerable<AdminFeedViewModel> Feeds { get; set; }
    }
}