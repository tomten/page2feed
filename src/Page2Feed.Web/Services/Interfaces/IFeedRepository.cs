using System.Collections.Generic;
using System.Threading.Tasks;
using Page2Feed.Core.Model;

namespace Page2Feed.Web.Services.Interfaces
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