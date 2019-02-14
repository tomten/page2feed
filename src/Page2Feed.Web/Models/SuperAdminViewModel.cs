using Page2Feed.Core.Model;
using System.Collections.Generic;

namespace Page2Feed.Web.Models
{
    public class SuperAdminViewModel
    {
        public List<User> Users { get; set; }
        public class User
        {
            public List<Feed> Feeds { get; set; }
            public string UserName { get; internal set; }
        }
    }
}