using System.Collections.Generic;
using System.Threading.Tasks;
using Page2Feed.Core.Model;

namespace Page2Feed.Core.Services.Interfaces
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

        Task Delete(
            string feedGroupName, 
            string feedName
            );

    }

}