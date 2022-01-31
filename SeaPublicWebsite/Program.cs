using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SeaPublicWebsite.Helpers;

namespace SeaPublicWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder webHostBuilder = Host.CreateDefaultBuilder(args);

            webHostBuilder.ConfigureAppConfiguration(ConfigureAppConfiguration);

            webHostBuilder.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
            
            return webHostBuilder;
        }

        private static void ConfigureAppConfiguration(HostBuilderContext builderContext, IConfigurationBuilder configBuilder)
        {
            Config.Build(configBuilder);
        }
    }
}