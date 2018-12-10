using System;
using System.Threading.Tasks;

namespace Page2Feed.Web.Services.Interfaces
{

    public interface IWebRepository
    {

        Task<string> GetContents(Uri uri);

    }

}