using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NLog;
using Page2Feed.Core.Services.Interfaces;

namespace Page2Feed.Web.Program
{

    // ReSharper disable once ClassNeverInstantiated.Global
    public class FeedBackgroundService : BackgroundService
    {

        private readonly IFeedService _feedService;
        private readonly ILogger _log;

        public FeedBackgroundService(IFeedService feedService)
        {
            _feedService = feedService;
            _log = LogManager.GetLogger("Feeds");
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken
            )
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _log.Trace("Running feed task (FeedHostedService.ExecuteAsync)...");
                    await _feedService.ProcessFeeds();
                    var wait = TimeSpan.FromMinutes(5);
                    _log.Trace($"Waiting {wait:g} for next run...");
                    await Task.Delay(wait, stoppingToken);
                    if (stoppingToken.IsCancellationRequested)
                    {
                        _log.Info("FeedHostedService.ExecuteAsync stopped while delaying on service execution cancellation token.");
                    }
                    _log.Trace("Done running feed task (FeedHostedService.ExecuteAsync).");
                }
                catch (Exception exception)
                {
                    _log.Error($"Error when executing feed task (FeedHostedService.ExecuteAsync): {exception}");
                }
            }
            _log.Info("FeedHostedService.ExecuteAsync exiting due to service execution cancellation token cancellation requested.");
        }

    }

}