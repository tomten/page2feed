using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Page2Feed.Web.Program
{

    public class WebHostRunner
    {

        public static void Main(string[] args)
        {
            WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Configurator>()
                .Build()
                .Run()
                ;
        }

    }

}
