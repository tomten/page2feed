using System.Collections.Generic;
using System.Threading.Tasks;
using Page2Feed.Core.Model;

namespace Page2Feed.Core.Services.Interfaces
{

    public interface IFeedRepository
    {

        Task<Feed> Get(
            string userName,
            string feedGroupName,
            string feedName
            );

        Task<Feed> Get(string id);

        Task Store(
            Feed feed
            );

        Task<IEnumerable<Feed>> GetAll();

        Task Delete(
            string userName,
            string feedGroupName,
            string feedName
            );

        Task Delete(
            string feedId
            );

        Task<string> GetFeedId(
            string userName,
            string feedGroupName,
            string feedName
            );

    }

}