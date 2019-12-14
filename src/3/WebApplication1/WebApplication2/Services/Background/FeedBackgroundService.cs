using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog;

namespace Page2Feed.Web.Services.Background
{

    // ReSharper disable once ClassNeverInstantiated.Global
    public class FeedBackgroundService : BackgroundService
    {

        private readonly FeedMonitor _feedMonitor;
        private readonly FeedService _feedService;
        private readonly ILogger _log;
        private readonly TimeSpan _feedCheckInterval;

        /// <exception cref="T:System.ArgumentException">When could not parse feed check interval FeedCheckInterval</exception>
        public FeedBackgroundService(
            FeedService feedService,
            IConfiguration configuration,
            FeedMonitor feedMonitor
            )
        {
            _feedMonitor = feedMonitor;
            _feedService = feedService;
            _log = LogManager.GetLogger("Feeds");
            if (!TimeSpan.TryParse(configuration["Page2Feed:FeedCheckInterval"], out _feedCheckInterval))
                throw new ArgumentException(
                    "Could not parse feed check interval",
                    "Page2Feed:FeedCheckInterval"
                    );
#if DEBUG
            _feedCheckInterval = TimeSpan.FromSeconds(5);
#endif
            _feedMonitor.SetNextFeedCheck(DateTimeOffset.Now + _feedCheckInterval);
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
                    _log.Trace($"Waiting {_feedCheckInterval:g} for next run...");
                    _feedMonitor.SetNextFeedCheck(DateTimeOffset.Now + _feedCheckInterval);
                    await Task.Delay(_feedCheckInterval, stoppingToken);
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