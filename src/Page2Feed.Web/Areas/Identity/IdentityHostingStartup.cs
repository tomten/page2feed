using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Page2Feed.Web.Areas.Identity.IdentityHostingStartup))]
namespace Page2Feed.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}