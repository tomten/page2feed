using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Page2Feed.Core;
using Page2Feed.Web.Services.Background;
using Formatting = System.Xml.Formatting;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Page2Feed.Web.App
{

    public class Page2FeedApp
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Page2FeedApp>();
                });

        public Page2FeedApp(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // TODO: AddDbContext

            // TODO: Identity?

            services.AddHostedService<FeedBackgroundService>();
            services.AddTransient<FeedService>();
            services.AddTransient<FeedRepository>(provider => new FeedRepository(Configuration["Page2Feed:FileFeedRepository:FeedBasePath"]));
            services.AddTransient<WebRepository>();
            services.AddTransient<Html2TextConverter>();
            services.AddSingleton<FeedMonitor>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}