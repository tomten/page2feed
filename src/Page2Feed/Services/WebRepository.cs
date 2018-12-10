using System;
using System.Net.Http;
using System.Threading.Tasks;
using Page2Feed.Services.Interfaces;

namespace Page2Feed.Services
{

    public class WebRepository : IWebRepository
    {

        private readonly HttpClient _httpClient;

        public WebRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetContents(Uri uri)
        {
            return await _httpClient.GetStringAsync(uri);
        }

    }

}