using System;
using System.Threading.Tasks;

namespace Page2Feed.Services.Interfaces
{

    public interface IWebRepository
    {

        Task<string> GetContents(Uri uri);

    }

}