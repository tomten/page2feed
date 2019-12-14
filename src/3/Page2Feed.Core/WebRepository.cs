using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Page2Feed.Web.App
{

    public class WebRepository 
    {

        private readonly HttpClient _httpClient;
#if DEBUG
        // When debugging, add random crap to fetched web contents to always provoke feed updates
        private readonly Random _random = new Random();
#endif

        public WebRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetContents(Uri uri)
        {
            var contents = await _httpClient.GetStringAsync(uri);
#if DEBUG
            // When debugging, add random crap to fetched web contents to always provoke feed updates
            contents += _random.Next(1000000, 9999999).ToString().Md5Hex();
#endif
            return contents;
        }

    }

}