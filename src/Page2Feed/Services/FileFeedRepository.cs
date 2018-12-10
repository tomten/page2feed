using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using Page2Feed.Model;
using Page2Feed.Services.Interfaces;

namespace Page2Feed.Services
{

    public class FileFeedRepository : IFeedRepository
    {

        private const string BasePath = @"d:\temp\feed\"; // TODO
        private readonly ILogger _log;

        public FileFeedRepository()
        {
            _log = LogManager.GetLogger("Feeds");
        }

        public async Task<Feed> Get(
            string feedGroupName,
            string feedName
            )
        {
            try
            {
                var filePath =
                    MakeFilePath(
                        feedGroupName,
                        feedName
                    );
                using (var fileStream = File.OpenText(filePath))
                {
                    var fileContents = await fileStream.ReadToEndAsync();
                    var feed = JsonConvert.DeserializeObject<Feed>(fileContents);
                    return feed;
                }
            }
            catch (Exception exception)
            {
                _log.Error($"Error when getting feed {feedGroupName}:{feedName}: {exception}");
                throw;
            }
        }

        private async Task<Feed> GetByFileName(
            string fileName
            )
        {
            try
            {
                _log.Trace($"Getting feed {fileName}...");
                var filePath =
                    Path.Combine(
                        BasePath,
                        fileName
                    );
                using (var fileStream = File.OpenText(filePath))
                {
                    var fileContents = await fileStream.ReadToEndAsync();
                    var feed = JsonConvert.DeserializeObject<Feed>(fileContents);
                    return feed;
                }
            }
            catch (Exception exception)
            {
                _log.Error($"Error when getting feed {fileName}: {exception}");
                throw;
            }
        }

        private string MakeFilePath(
            string feedGroupName,
            string feedName
            )
        {
            var fileName =
                MakeFileName(
                    feedGroupName,
                    feedName
                );
            var filePath =
                Path.Combine(
                    BasePath,
                    fileName
                );
            return filePath;
        }

        public async Task Store(
            Feed feedToStore
            )
        {
            var filePath =
                MakeFilePath(
                    feedToStore.Group,
                    feedToStore.Name
                );
            using (var feedStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (var feedWriter = new StreamWriter(feedStream))
                {
                    var feed = JsonConvert.SerializeObject(
                        feedToStore,
                        Formatting.Indented
                        );
                    await feedWriter.WriteAsync(feed);
                }
            }
        }

        public async Task<IEnumerable<Feed>> GetAll()
        {
            var feeds = new List<Feed>();
            var filePaths = Directory.EnumerateFiles(BasePath);
            foreach (var filePath in filePaths)
            {
                var fileName = Path.GetFileName(filePath);
                var feed = await GetByFileName(fileName);
                feeds.Add(feed);
            }
            return feeds;
        }

        private string MakeFileName(
            string feedGroupName,
            string feedName
            )
        {
            return $"{feedGroupName}{feedName}".Md5Hex();
        }

    }

}