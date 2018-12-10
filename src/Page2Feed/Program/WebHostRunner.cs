using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Page2Feed.Program
{

    // ReSharper disable once ClassNeverInstantiated.Global
    public class WebHostRunner
    {

        public static void Main(string[] args)
        {
            WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Configurator>()
                .Build()
                .Run();
        }

    }

}
