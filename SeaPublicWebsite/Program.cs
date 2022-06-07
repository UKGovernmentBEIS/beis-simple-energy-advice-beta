using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Network;

namespace SeaPublicWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();
            
            var builder = WebApplication.CreateBuilder(args);
            
            var startup = new Startup(builder.Configuration, builder.Environment);
            startup.ConfigureServices(builder.Services);

            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .WriteTo.Console()
                .Enrich.FromLogContext());

            var app = builder.Build();

            startup.Configure(app, app.Environment);

            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder webHostBuilder = Host.CreateDefaultBuilder(args);

            webHostBuilder.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
            
            return webHostBuilder;
        }
    }
}