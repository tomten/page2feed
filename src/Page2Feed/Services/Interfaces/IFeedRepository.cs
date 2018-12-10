using System.Collections.Generic;
using System.Threading.Tasks;
using Page2Feed.Model;

namespace Page2Feed.Services.Interfaces
{

    public interface IFeedRepository
    {

        Task<Feed> Get(
            string feedGroupName,
            string feedName
            );

        Task Store(
            Feed feed
            );

        Task<IEnumerable<Feed>> GetAll();

    }

}