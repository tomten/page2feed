using System;
using System.Diagnostics;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Page2Feed.Core.Services;
using Page2Feed.Core.Services.Interfaces;
using Page2Feed.Web.Program.Services;

namespace Page2Feed.Web.Program
{

    public class Page2Feed
    {

        public Page2Feed(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static void Main(string[] args)
        {
            try
            {
                WebHost
                    .CreateDefaultBuilder(args)
                    .UseStartup<Page2Feed>()
                    .Build()
                    .Run()
                    ;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Debug.WriteLine(exception);
                throw;
            }
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHostedService<FeedBackgroundService>();
            services.AddTransient<IFeedService, FeedService>();
            services.AddTransient<IFeedRepository, FileFeedRepository>(provider => new FileFeedRepository(Configuration["Page2Feed:FileFeedRepository:FeedBasePath"]));
            services.AddTransient<IWebRepository, WebRepository>();
            services.AddTransient<IHtml2TextConverter, Html2TextConverter>();
            services.AddSingleton<IFeedMonitor, FeedMonitor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }

}