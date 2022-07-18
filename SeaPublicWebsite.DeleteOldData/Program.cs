using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeaPublicWebsite.Data;

namespace SeaPublicWebsite.DeleteOldData
{
    class Program
    {
        static void Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            DeleteOldData(host.Services);
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddScoped<IDataAccessProvider, DataAccessProvider>()
                        .AddDbContext<SeaDbContext>(opt =>
                        {
                            IConfiguration configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: true,
                                    reloadOnChange: true)
                                .AddEnvironmentVariables()
                                .AddCommandLine(args)
                                .Build();
                            //TODO: add db connection string for prod env
                            var databaseConnectionString = configuration.GetConnectionString("PostgreSQLConnection");
                            opt.UseNpgsql(databaseConnectionString);
                        });
                });
        }

        static void DeleteOldData(IServiceProvider services)
        {
            using IServiceScope serviceScope = services.CreateScope();
            services.GetRequiredService<IDataAccessProvider>().DeleteOldPropertyData();
        }
    }
}