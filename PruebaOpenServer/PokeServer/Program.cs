using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PokeServices.PokedexServices;

namespace PokeServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IWebHost webHost = CreateWebHostBuilder(args).Build();

            //Instantiate the service
            var profilerService = webHost.Services.GetRequiredService<PokedexProfilerService>();
            await profilerService.InitAsync();

            // Run the WebHost, and start accepting requests
            await webHost.RunAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
